using Npgsql;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;

namespace StudyBuddy.Persistence
{
    class GameBadgeRepository : SqlRepositoryBase, IGameBadgeRepository
    {
        public GameBadgeRepository(string connection_string) : base(connection_string)
        {
            if (!TableExists("game_badges")) 
            {
                CreateBadgesTable();
            }

            if (!TableExists("game_badge_challenges")) 
            {
                CreateGameBadgeChallengesTable();
            }
        }

        private void CreateBadgesTable() 
        {
            string sql = "create table game_badges (" +
                "id serial primary key, " +
                "name varchar(100) not null, " +
                "owner_id int not null, " + 
                "created date not null, " +
                "required_coverage numeric(2,2) not null)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateGameBadgeChallengesTable() 
        {
            string sql = "create table game_badge_challenges (" +
                "game_badge int not null, " + 
                "challenge int not null)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
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

        public GameBadge ById(int id)
        {
            string sql = "SELECT id,name,owner_id,created,required_coverage " +
                "FROM game_badges where id=:id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", id);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return FromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<GameBadge> All(int from = 0, int max = 1000)
        {
            string sql = "SELECT id,name,owner_id,created,required_coverage " +
                "FROM game_badges order by created limit :max offset :from";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":from", from);
                    cmd.Parameters.AddWithValue(":max", max);
                    var result = new List<GameBadge>();

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var obj = FromReader(reader);
                            result.Add(obj);
                        }
                    }

                    return result;
                }
            }
        }

        public void Insert(GameBadge obj)
        {
            string sql = "insert into game_badges (name,owner_id,created," + 
                "required_coverage) values (:name,:owner_id,:created," +
                ":required_coverage) RETURNING id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":name", obj.Name);
                    cmd.Parameters.AddWithValue(":owner_id", obj.OwnerID);
                    cmd.Parameters.AddWithValue(":created", obj.Created);
                    cmd.Parameters.AddWithValue(":required_coverage", obj.RequiredCoverage);
                    obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(GameBadge obj)
        {
            string sql = "update game_badges set name=:name,owner_id=:owner_id," +
                "created=:created,required_coverage=:required_coverage where id=:id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", obj.ID);
                    cmd.Parameters.AddWithValue(":name", obj.Name);
                    cmd.Parameters.AddWithValue(":owner_id", obj.OwnerID);
                    cmd.Parameters.AddWithValue(":created", obj.Created);
                    cmd.Parameters.AddWithValue(":required_coverage", obj.RequiredCoverage);
                    cmd.ExecuteNonQuery();
                }
            }
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
            string sql = "delete from game_badges where id=:id";
            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            sql = "delete from game_badge_challenges where game_badge=:id";
            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddChallenge(int badge_id, int challenge_id)
        {
            string sql = "insert into game_badge_challenges " +
                    "(game_badge,challenge) values (:badge_id,:challenge_id)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":badge_id", badge_id);
                    cmd.Parameters.AddWithValue(":challenge_id", challenge_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveChallenge(int badge_id, int challenge_id)
        {
            string sql = "delete from game_badge_challenges where " +
                "game_badge=:badge_id and challenge=:challenge_id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":badge_id", badge_id);
                    cmd.Parameters.AddWithValue(":challenge_id", challenge_id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}