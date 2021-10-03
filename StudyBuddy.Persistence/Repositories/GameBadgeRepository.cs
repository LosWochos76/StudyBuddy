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
            CreateGameBadgeChallengesTable();
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
            qh.Delete("game_badge_challenges", "game_badge", id);
        }

        public void AddChallenge(int badge_id, int challenge_id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":badge_id", badge_id);
            qh.AddParameter(":challenge_id", challenge_id);
            qh.ExecuteNonQuery(
                "insert into game_badge_challenges " +
                "(game_badge,challenge) values (:badge_id,:challenge_id)");
        }

        public void RemoveChallenge(int badge_id, int challenge_id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":badge_id", badge_id);
            qh.AddParameter(":challenge_id", challenge_id);
            qh.ExecuteNonQuery(
                "delete from game_badge_challenges where " +
                "game_badge=:badge_id and challenge=:challenge_id");
        }

        public void DeleteChallenges(int badge_id)
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);
            qh.AddParameter(":badge_id", badge_id);
            qh.ExecuteNonQuery(
                "delete from game_badge_challenges where game_badge=:badge_id");
        }

        public void AddChallenges(GameBadgeChallenge[] challenges)
        {
            foreach (var obj in challenges)
                AddChallenge(obj.GameBadgeId, obj.ChallengeId);
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
        }

        private void CreateGameBadgeChallengesTable()
        {
            var qh = new QueryHelper<GameBadge>(connection_string, FromReader);

            if (!qh.TableExists("game_badge_challenges"))
                qh.ExecuteNonQuery(
                    "create table game_badge_challenges (" +
                    "game_badge int not null, " +
                    "challenge int not null)");
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
    }
}