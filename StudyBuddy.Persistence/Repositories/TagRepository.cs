using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class TagRepository : ITagRepository
    {
        private readonly string connection_string;

        public TagRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
            CreateChallengeTagsTable();
            CreateBadgesTagsTable();
        }

        public Tag ById(int id)
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name FROM tags where id=:id");
        }

        public Tag ByName(string name)
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);
            qh.AddParameter(":name", name.ToLower());
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name FROM tags where name=:name");
        }

        public int Count()
        {
            var qh = new QueryHelper<Tag>(connection_string);
            return qh.GetCount("tags");
        }

        public IEnumerable<Tag> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);
            qh.AddParameters(new {from, max});
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name FROM tags order by name limit :max offset :from");
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.Delete("tags_challenges", "tag_id", id);
            qh.Delete("tags", "id", id);
        }

        public void Insert(Tag obj)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new {name = obj.Name.ToLower().Replace("#", "")});
            obj.ID = qh.ExecuteScalar(
                "insert into tags (name) values (:name) RETURNING id");
        }

        public void Update(Tag obj)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new {id = obj.ID, name = obj.Name.ToLower().Replace("#", "")});
            qh.ExecuteNonQuery("update tags set name=:name where id=:id");
        }

        public void Save(Tag obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void RemoveAllTagsFromChallenge(int challenge_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new {challenge_id});
            qh.ExecuteNonQuery(
                "delete from tags_challenges where challenge_id=:challenge_id");
        }

        public void AddTagForChallenge(int tag_id, int challenge_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new {tag_id, challenge_id});
            qh.ExecuteNonQuery(
                "insert into tags_challenges(challenge_id, tag_id) " +
                "values(:challenge_id, :tag_id) ON CONFLICT DO NOTHING;");
        }

        public void RemoveAllTagsFromBadge(int badge_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new { badge_id });
            qh.ExecuteNonQuery("delete from tags_badges where badge_id=:badge_id");
        }

        public void AddTagForBadge(int tag_id, int badge_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new { tag_id, badge_id });
            qh.ExecuteNonQuery(
                "insert into tags_badges(badge_id, tag_id) " +
                "values(:badge_id, :tag_id) ON CONFLICT DO NOTHING;");
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<Tag>(connection_string, FromReader);

            if (!qh.TableExists("tags"))
                qh.ExecuteNonQuery(
                    "create table tags (" +
                    "id serial primary key, " +
                    "name varchar(50) not null);");
        }

        private void CreateChallengeTagsTable()
        {
            var rh = new RevisionHelper(connection_string, "tags_challenges");
            var qh = new QueryHelper<Tag>(connection_string);

            if (!qh.TableExists("tags_challenges"))
                qh.ExecuteNonQuery(
                    "create table tags_challenges (" +
                    "challenge_id int not null, " +
                    "tag_id int not null," +
                    "unique (challenge_id, tag_id))");
        }

        private void CreateBadgesTagsTable()
        {
            var rh = new RevisionHelper(connection_string, "tags_badges");
            var qh = new QueryHelper<Tag>(connection_string);

            if (!qh.TableExists("tags_badges"))
                qh.ExecuteNonQuery(
                    "create table tags_badges (" +
                    "badge_id int not null, " +
                    "tag_id int not null," +
                    "unique (badge_id, tag_id))");
        }

        private Tag FromReader(NpgsqlDataReader reader)
        {
            var obj = new Tag();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            return obj;
        }
    }
}