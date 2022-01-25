using System;
using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class GameBadgeRepository : IGameBadgeRepository
    {
        private readonly string connection_string;

        public GameBadgeRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateBadgesTable();
            CreateBadgesUserTable();
        }

        public GameBadge ById(int id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":id", id);

            return qh.ExecuteQueryToSingleObject("select id,name,owner_id,created," +
                "required_coverage,description,tags_of_badge(id) FROM game_badges where id=:id");
        }

        public IEnumerable<GameBadge> All(GameBadgeFilter filter)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var sql = "select id,name,owner_id,created,required_coverage,description,tags_of_badge(id) " +
                "FROM game_badges where true";

            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId);
                sql += " and owner_id=:owner_id";
            }

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql += " and (name ilike :search_text or description ilike :search_text or tags_list ilike :search_text)";
            }

            sql += " order by created, name limit :max offset :from";

            return qh.ExecuteQueryToObjectList(sql);
        }

        public int GetCount(GameBadgeFilter filter)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            var sql = "select count(*) FROM game_badges where true";

            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId);
                sql += " and owner_id=:owner_id";
            }

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql += " and (name ilike :search_text or description ilike :search_text or tags_list ilike :search_text)";
            }

            return qh.ExecuteQueryToSingleInt(sql);
        }

        public void Insert(GameBadge obj)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":required_coverage", obj.RequiredCoverage);
            qh.AddParameter(":description", obj.Description);

            obj.ID = qh.ExecuteScalar(
                "insert into game_badges (name,owner_id,created,required_coverage,description) " +
                "values (:name,:owner_id,:created,:required_coverage,:description) RETURNING id");
        }

        public void Update(GameBadge obj)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":required_coverage", obj.RequiredCoverage);
            qh.AddParameter(":description", obj.Description);

            qh.ExecuteNonQuery(
                "update game_badges set name=:name,owner_id=:owner_id,created=:created," +
                "required_coverage=:required_coverage,description=:description where id=:id");
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
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.Delete("game_badges", "id", id);
        }

        public IEnumerable<GameBadge> GetBadgesForChallenge(int challenge_id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":challenge_id", challenge_id);

            var sql = "select distinct gb.id,gb.name,gb.owner_id,gb.created,gb.required_coverage,gb.description,tags_of_badge(gb.id) from game_badges gb " +
                "inner join tags_badges tb on gb.id = tb.badge_id " +
                "inner join tags_challenges tc on tc.tag_id = tb.tag_id " +
                "where tc.challenge_id=:challenge_id " +
                "order by created desc, name";

            return qh.ExecuteQueryToObjectList(sql);
        }

        // Get the success rate of a specific user for a certain badge
        public BadgeSuccessRate GetSuccessRate(int badge_id, int user_id)
        {
            var qh = new QueryHelper<BadgeSuccessRate>(connection_string, BadgeSuccessRateFromReader);
            qh.AddParameter(":badge_id", badge_id);
            qh.AddParameter(":user_id", user_id);

            var sql = "select " +
                ":badge_id as badge_id, " +
                ":user_id as user_id, " +
                "(select count(*) as overall_challenge_count from " +
                "   (select id from challenges " +
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

            return qh.ExecuteQueryToSingleObject(sql);
        }

        public void AddBadgeToUser(int user_id, int badge_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameters(new { user_id, badge_id, created = DateTime.Now.Date });
            qh.ExecuteNonQuery(
                "insert into users_badges(user_id, badge_id, created) " +
                "values(:user_id, :badge_id, :created) ON CONFLICT DO NOTHING;");
        }

        public void RemoveBadgeFromUser(int user_id, int badge_id)
        {
            var qh = new QueryHelper<Tag>(connection_string);
            qh.AddParameter(":user_id", user_id);
            qh.AddParameter(":badge_id", badge_id);
            qh.ExecuteNonQuery("delete from users_badges where user_id=:user_id and badge_id=:badge_id");
        }

        public IEnumerable<GameBadge> GetBadgesOfUser(int user_id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":user_id", user_id);

            var sql = "select id,name,owner_id,game_badges.created,required_coverage,description,tags_of_badge(id) " +
                "from game_badges " +
                "inner join users_badges on id=badge_id " +
                "where user_id=:user_id " +
                "order by users_badges.created desc";

            return qh.ExecuteQueryToObjectList(sql);
        }

        private void CreateBadgesTable()
        {
            var rh = new RevisionHelper(connection_string, "badges");
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);

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
        }

        private void CreateBadgesUserTable()
        {
            var rh = new RevisionHelper(connection_string, "users_badges");
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);

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

        private GameBadge FromReader(NpgsqlDataReader reader)
        {
            var obj = new GameBadge();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.OwnerID = reader.GetInt32(2);
            obj.Created = reader.GetDateTime(3);
            obj.RequiredCoverage = reader.GetDouble(4);
            obj.Description = reader.IsDBNull(5) ? "" : reader.GetString(5);
            obj.Tags = reader.IsDBNull(6) ? "" : reader.GetString(6);
            return obj;
        }

        private BadgeSuccessRate BadgeSuccessRateFromReader(NpgsqlDataReader reader)
        {
            var obj = new BadgeSuccessRate();
            obj.BadgeId = reader.GetInt32(0);
            obj.UserId = reader.GetInt32(1);
            obj.OverallChallengeCount = reader.GetInt32(2);
            obj.AcceptedChallengeCount = reader.GetInt32(3);
            return obj;
        }
    }
}