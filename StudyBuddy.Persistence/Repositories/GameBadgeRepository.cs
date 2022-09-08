using System;
using System.Collections.Generic;
using System.Text;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class GameBadgeRepository : IGameBadgeRepository
    {
        private readonly string connection_string;
        private readonly GameBadgeConverter converter = new GameBadgeConverter();

        public GameBadgeRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateBadgesTable();
            CreateBadgesUserTable();
        }

        public GameBadge ById(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":id", id);
            var set = qh.ExecuteQuery("select id,name,owner_id,created," +
                "required_coverage,description,iconkey,tags_of_badge(id) FROM game_badges where id=:id");
            return converter.Single(set);
        }

        public IEnumerable<GameBadge> All(GameBadgeFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);
            var sql = new StringBuilder("select id,name,owner_id,created,required_coverage,description,iconkey,tags_of_badge(id) FROM game_badges where true");
            ApplyFilter(qh, sql, filter);
            sql.Append(" order by created, name limit :max offset :from");
            var set = qh.ExecuteQuery(sql.ToString());
            return converter.Multiple(set);
        }

        public int GetCount(GameBadgeFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            var sql = new StringBuilder("select count(*) FROM game_badges where true");
            ApplyFilter(qh, sql, filter);
            return qh.ExecuteQueryToSingleInt(sql.ToString());
        }

        private void ApplyFilter(QueryHelper qh, StringBuilder sql, GameBadgeFilter filter)
        {
            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId);
                sql.Append(" and owner_id=:owner_id");
            }

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql.Append(" and (name ilike :search_text or description ilike :search_text or tags_of_badge(id) ilike :search_text)");
            }
        }

        public IEnumerable<GameBadge> GetReceivedBadgesOfUser(int user_id, GameBadgeFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", user_id);
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);

            var sql = new StringBuilder("select id,name,owner_id,game_badges.created," +
                "required_coverage,description,iconkey,tags_of_badge(id),users_badges.created as received " +
                "from game_badges " +
                "inner join users_badges on id=badge_id " +
                "where user_id=:user_id");

            ApplyFilter(qh, sql, filter);
            sql.Append(" order by users_badges.created desc limit :max offset :from");
            var set = qh.ExecuteQuery(sql.ToString());
            return converter.Multiple(set);
        }

        public void Insert(GameBadge obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":required_coverage", obj.RequiredCoverage);
            qh.AddParameter(":description", obj.Description);
            qh.AddParameter(":iconkey", obj.IconKey);

            obj.ID = qh.ExecuteScalar(
                "insert into game_badges (name,owner_id,created,required_coverage,description,iconkey) " +
                "values (:name,:owner_id,:created,:required_coverage,:description,:iconkey) RETURNING id");
        }

        public void Update(GameBadge obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":required_coverage", obj.RequiredCoverage);
            qh.AddParameter(":description", obj.Description);
            qh.AddParameter(":iconkey", obj.IconKey);

            qh.ExecuteNonQuery(
                "update game_badges set name=:name,owner_id=:owner_id,created=:created," +
                "required_coverage=:required_coverage,description=:description,iconkey=:iconkey where id=:id");
        }

        public void Save(GameBadge obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.Delete("game_badges", "id", id);
        }

        public IEnumerable<GameBadge> GetBadgesForChallenge(int challenge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":challenge_id", challenge_id);

            var sql = "select distinct gb.id,gb.name,gb.owner_id,gb.created,gb.required_coverage,gb.description,tags_of_badge(gb.id) from game_badges gb " +
                "inner join tags_badges tb on gb.id = tb.badge_id " +
                "inner join tags_challenges tc on tc.tag_id = tb.tag_id " +
                "where tc.challenge_id=:challenge_id " +
                "order by created desc, name";

            var set = qh.ExecuteQuery(sql);
            return converter.Multiple(set);
        }

        // Get the success rate of a specific user for a certain badge
        public BadgeSuccessRate GetSuccessRate(int badge_id, int user_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":badge_id", badge_id);
            qh.AddParameter(":user_id", user_id);

            var sql = "select " +
                ":badge_id as badge_id, " +
                ":user_id as user_id, " +
                "(select count(*) as overall_challenge_count from " +
                "   (select distinct id from challenges " +
                "       inner join tags_challenges tc on id = tc.challenge_id " +
                "       inner join tags_badges tb on tc.tag_id = tb.tag_id " +
                "       where tb.badge_id = :badge_id " +
                "   ) as a " +
                ")," +
                "(select count(*) as accepted_challenge_count from" +
                "   (select distinct id from challenges " +
                "       inner join tags_challenges tc on id = tc.challenge_id " +
                "       inner join tags_badges tb on tc.tag_id = tb.tag_id " +
                "       inner join challenge_acceptance ca on ca.challenge_id = id " +
                "       where tb.badge_id = :badge_id and ca.user_id = :user_id " +
                "   ) as b " +
                "); ";

            var converter = new BadgeSuccessRateConverter();
            var set = qh.ExecuteQuery(sql);
            return converter.Single(set);
        }

        public void AddBadgeToUser(int user_id, int badge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameters(new { user_id, badge_id, created = DateTime.Now.Date });
            qh.ExecuteNonQuery(
                "insert into users_badges(user_id, badge_id, created) " +
                "values(:user_id, :badge_id, :created) ON CONFLICT DO NOTHING;");
        }

        public void RemoveBadgeFromUser(int user_id, int badge_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", user_id);
            qh.AddParameter(":badge_id", badge_id);
            qh.ExecuteNonQuery("delete from users_badges where user_id=:user_id and badge_id=:badge_id");
        }

        public void RemoveAllBadgesFromUser(int user_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", user_id);
            qh.ExecuteNonQuery("delete from users_badges where user_id=:user_id");
        }

        private void CreateBadgesTable()
        {
            var rh = new RevisionHelper(connection_string, "badges");
            var qh = new QueryHelper(connection_string);

            if (!qh.TableExists("game_badges"))
            {
                qh.ExecuteNonQuery(
                    "create table game_badges (" +
                    "id serial primary key, " +
                    "name varchar(100) not null, " +
                    "owner_id int not null, " +
                    "created date not null, " +
                    "required_coverage numeric(3,2) not null," +
                    "description text)");

                rh.SetRevision(3);
            }

            if (rh.GetRevision() == 1)
            {
                qh.ExecuteNonQuery(
                    "ALTER TABLE game_badges " +
                    "ALTER COLUMN required_coverage TYPE numeric(3,2)");

                rh.SetRevision(2);
            }

            if (qh.TableExists("game_badge_challenges"))
            {
                qh.ExecuteNonQuery("drop table game_badge_challenges");
            }

            if (rh.GetRevision() == 2)
            {
                qh.ExecuteNonQuery(
                    "ALTER TABLE game_badges " +
                    "ADD COLUMN description text");

                rh.SetRevision(3);
            }

            if (rh.GetRevision() == 3)
            {
                qh.ExecuteNonQuery(
                    "ALTER TABLE game_badges " +
                    "ADD COLUMN iconkey varchar(30)");
                rh.SetRevision(4);
            }
        }

        private void CreateBadgesUserTable()
        {
            var rh = new RevisionHelper(connection_string, "users_badges");
            var qh = new QueryHelper(connection_string);

            if (!qh.TableExists("users_badges"))
            {
                qh.ExecuteNonQuery(
                    "create table users_badges (" +
                    "user_id int, " +
                    "badge_id int," +
                    "created date not null," +
                    "unique (user_id, badge_id))");
            }
        }
    }
}