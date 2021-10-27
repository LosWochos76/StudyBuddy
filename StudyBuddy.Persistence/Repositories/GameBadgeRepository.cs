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
        }

        public GameBadge ById(int id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,name,owner_id,created,required_coverage " +
                "FROM game_badges where id=:id");
        }

        public IEnumerable<GameBadge> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,owner_id,created,required_coverage " +
                "FROM game_badges order by created,name limit :max offset :from");
        }

        public IEnumerable<GameBadge> OfOwner(int owner_id, int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":owner_id", owner_id);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,name,owner_id,created,required_coverage " +
                "FROM game_badges where owner_id=:owner_id order by created,name limit :max offset :from");
        }

        public void Insert(GameBadge obj)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":required_coverage", obj.RequiredCoverage);
            obj.ID = qh.ExecuteScalar(
                "insert into game_badges (name,owner_id,created," +
                "required_coverage) values (:name,:owner_id,:created," +
                ":required_coverage) RETURNING id");
        }

        public void Update(GameBadge obj)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":required_coverage", obj.RequiredCoverage);
            qh.ExecuteNonQuery(
                "update game_badges set name=:name,owner_id=:owner_id," +
                "created=:created,required_coverage=:required_coverage where id=:id");
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
            qh.Delete("tags_badges", "badge_id", id);
        }

        // Get all of the badges that belong to the given challenge
        public IEnumerable<GameBadge> GetBadgesForChallenge(int challenge_id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":challenge_id", challenge_id);
            return qh.ExecuteQueryToObjectList(
                "select distinct id,name,owner_id,created,required_coverage from game_badges " +
                "inner join tags_badges tb on id=tb.badge_id " +
                "inner join tags_challenges tc on tb.tag_id = tc.tag_id " +
                "where challenge_id=:challenge_id order by created,name");
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
                    "required_coverage numeric(3,2) not null)");

                rh.SetRevision(2);
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
        }

        private GameBadge FromReader(NpgsqlDataReader reader)
        {
            var obj = new GameBadge();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.OwnerID = reader.GetInt32(2);
            obj.Created = reader.GetDateTime(3);
            obj.RequiredCoverage = reader.GetDouble(4);
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