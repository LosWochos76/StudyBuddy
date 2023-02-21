using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    internal class TagRepository : ITagRepository
    {
        private readonly string connection_string;
        private readonly TagConverter converter = new TagConverter();

        public TagRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
            CreateChallengeTagsTable();
            CreateBadgesTagsTable();
        }

        public Tag ById(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":id", id);
            var set = qh.ExecuteQuery(
                "SELECT id,created,name FROM tags where id=:id");

            return converter.Single(set);
        }

        public Tag ByName(string name)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":name", name.ToLower());
            var set = qh.ExecuteQuery(
                "SELECT id,created,name FROM tags where name=:name");

            return converter.Single(set);
        }

        public int GetCount(TagFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            return qh.GetCount("tags");
        }

        public IEnumerable<Tag> All(TagFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);

            var sql = qh.ExecuteQuery(
                "SELECT id,created,name FROM tags order by name limit :max offset :from");

            return converter.Multiple(sql);
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.Delete("tags_challenges", "tag_id", id);
            qh.Delete("tags", "id", id);
        }

        public void Insert(Tag obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":name", obj.Name.ToLower().Replace("#", ""));
            qh.AddParameter(":created", obj.Created);
            obj.ID = qh.ExecuteScalar("insert into tags (created,name) values (:created,:name) RETURNING id");
        }

        public void Update(Tag obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":name", obj.Name.ToLower().Replace("#", ""));
            qh.ExecuteNonQuery("update tags set name=:name where id=:id");
        }

        public void Save(Tag obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void AddTagForChallenge(int tag_id, int challenge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":tag_id", tag_id);
            qh.AddParameter(":challenge_id", challenge_id);
            qh.ExecuteNonQuery(
                "insert into tags_challenges(challenge_id, tag_id) " +
                "values(:challenge_id, :tag_id) ON CONFLICT DO NOTHING;");
        }

        public IEnumerable<Tag> ForChallenge(int challenge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":challenge_id", challenge_id);
            var sql = qh.ExecuteQuery(
                "select id,created,name from tags inner join tags_challenges on tags_challenges.tag_id=id where challenge_id=:challenge_id");
            return converter.Multiple(sql);
        }

        public void RemoveAllTagsFromChallenge(int challenge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameters(new { challenge_id });
            qh.ExecuteNonQuery(
                "delete from tags_challenges where challenge_id=:challenge_id");
        }

        public void AddTagForBadge(int tag_id, int badge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameters(new { tag_id, badge_id });
            qh.ExecuteNonQuery(
                "insert into tags_badges(badge_id, tag_id) " +
                "values(:badge_id, :tag_id) ON CONFLICT DO NOTHING;");
        }

        public IEnumerable<Tag> ForBadge(int badge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":badge_id", badge_id);
            var sql = qh.ExecuteQuery(
                "select id,created,name from tags inner join tags_badges on tags_badges.tag_id=id where badge_id=:badge_id");
            return converter.Multiple(sql);
        }

        public void RemoveAllTagsFromBadge(int badge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameters(new { badge_id });
            qh.ExecuteNonQuery("delete from tags_badges where badge_id=:badge_id");
        }

        private void CreateTable()
        {
            var rh = new RevisionHelper(connection_string, "tags");
            var qh = new QueryHelper(connection_string);

            if (!qh.TableExists("tags"))
            {
                qh.ExecuteNonQuery(
                    "create table tags (" +
                    "id serial primary key, " +
                    "created date not null, " +
                    "name varchar(50) not null);");
                rh.SetRevision(2);
            }

            if (rh.GetRevision() == 1)
            {
                qh.ExecuteNonQuery("ALTER TABLE tags ADD COLUMN created date DEFAULT '2000-01-01'");
                rh.SetRevision(2);
            }
        }

        private void CreateChallengeTagsTable()
        {
            var rh = new RevisionHelper(connection_string, "tags_challenges");
            var qh = new QueryHelper(connection_string);

            if (!qh.TableExists("tags_challenges"))
                qh.ExecuteNonQuery(
                    "create table tags_challenges (" +
                    "challenge_id int not null, " +
                    "tag_id int not null," +
                    "unique (challenge_id, tag_id))");

            qh.ExecuteNonQuery("begin;\n" +
                "SELECT pg_advisory_xact_lock(7949957698783404873);\n" +
                "create or replace function tags_of_challenge(challenge_id_param int)\n" +
                "returns varchar\n" +
                "language plpgsql\n" +
                "as $$\n" +
                "declare\n" +
                "   tag_list varchar;\n" +
                "begin\n" +
                "   select string_agg('#' || name, ' ') into tag_list from tags_challenges\n" +
                "   inner join tags on tags_challenges.tag_id = tags.id\n" +
                "   where tags_challenges.challenge_id = challenge_id_param;\n" +
                "   return tag_list;\n" +
                "end;$$;\n" +
                "commit;");
        }

        private void CreateBadgesTagsTable()
        {
            var rh = new RevisionHelper(connection_string, "tags_badges");
            var qh = new QueryHelper(connection_string);

            if (!qh.TableExists("tags_badges"))
                qh.ExecuteNonQuery(
                    "create table tags_badges (" +
                    "badge_id int not null, " +
                    "tag_id int not null," +
                    "unique (badge_id, tag_id))");

            qh.ExecuteNonQuery("begin;\n" +
                "SELECT pg_advisory_xact_lock(3040531610174894300);\n" +
                "create or replace function tags_of_badge(badge_id_param int)\n" +
                "returns varchar\n" +
                "language plpgsql\n" +
                "as $$\n" +
                "declare\n" +
                "   tag_list varchar;\n" +
                "begin\n" +
                "   select string_agg('#' || name, ' ') into tag_list from tags_badges\n" +
                "   inner join tags on tags_badges.tag_id = tags.id\n" +
                "   where tags_badges.badge_id = badge_id_param;\n" +
                "   return tag_list;\n" +
                "end;$$;\n" +
                "commit;");
        }
    }
}