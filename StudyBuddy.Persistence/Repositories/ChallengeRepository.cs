using Npgsql;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;

namespace StudyBuddy.Persistence
{
    class ChallengeRepository : SqlRepositoryBase, IChallengeRepository
    {
        public ChallengeRepository(string connection_string) : base(connection_string)
        {
            if (!TableExists("challenges")) 
            {
                CreateTable();
            }
        }

        private void CreateTable() 
        {
            string sql = "create table challenges (" +
                "id serial primary key, " +
                "name varchar(100) not null, " +
                "description text, " +
                "points smallint not null, " +
                "validity_start date not null, " +
                "validity_end date not null, " +
                "category smallint not null, " +
                "owner_id int not null, " + 
                "created date not null, " +
                "prove smallint not null, " + 
                "target_audience text," + 
                "series_parent_id int)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private Challenge FromReader(NpgsqlDataReader reader)
        {
            var obj = new Challenge();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.Description = reader.IsDBNull(2) ? null : reader.GetString(2);
            obj.Points = reader.GetInt32(3);
            obj.ValidityStart = reader.GetDateTime(4);
            obj.ValidityEnd = reader.GetDateTime(5);
            obj.Category = (ChallengeCategory)reader.GetInt32(6);
            obj.OwnerID = reader.GetInt32(7);
            obj.Created = reader.GetDateTime(8);
            obj.Prove = (ChallengeProve)reader.GetInt32(9);
            obj.TargetAudience = reader.IsDBNull(10) ? null : reader.GetString(10);
            obj.SeriesParentID = reader.IsDBNull(11) ? null : reader.GetInt32(11);
            return obj;
        }

        public Challenge ById(int id)
        {
            string sql = "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,target_audience,series_parent_id " +
                "FROM challenges where id=:id";

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

        public IEnumerable<Challenge> All(int from = 0, int max = 1000)
        {
            string sql = "SELECT id,name,description,points,validity_start,validity_end," +
                "category,owner_id,created,prove,target_audience,series_parent_id " +
                "FROM challenges order by created limit :max offset :from";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":from", from);
                    cmd.Parameters.AddWithValue(":max", max);
                    var result = new List<Challenge>();

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

        private void Insert(Challenge obj)
        {
            string sql = "insert into challenges (name,description,points,"  + 
                "validity_start,validity_end,category,owner_id,created,prove," + 
                "target_audience,series_parent_id) values " +
                "(:name,:description,:points,:validity_start,:validity_end,:category," +
                ":owner_id,:created,:prove,:target_audience,:series_parent_id) RETURNING id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":name", obj.Name);
                    cmd.Parameters.AddWithValue(":description", string.IsNullOrEmpty(obj.Description) ? DBNull.Value : obj.Description);
                    cmd.Parameters.AddWithValue(":points", obj.Points);
                    cmd.Parameters.AddWithValue(":validity_start", obj.ValidityStart);
                    cmd.Parameters.AddWithValue(":validity_end", obj.ValidityEnd);
                    cmd.Parameters.AddWithValue(":category", (int)obj.Category);
                    cmd.Parameters.AddWithValue(":owner_id", obj.OwnerID);
                    cmd.Parameters.AddWithValue(":created", obj.Created);
                    cmd.Parameters.AddWithValue(":prove", (int)obj.Prove);
                    cmd.Parameters.AddWithValue(":target_audience", string.IsNullOrEmpty(obj.TargetAudience) ? DBNull.Value : obj.TargetAudience);
                    cmd.Parameters.AddWithValue(":series_parent_id", obj.SeriesParentID.HasValue ? obj.SeriesParentID.Value : DBNull.Value);
                    obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private void Update(Challenge obj)
        {
            string sql = "update challenges set name=:name,description=:description,points=:points," + 
                "validity_start=:validity_start,validity_end=:validity_end,category=:category," + 
                "owner_id=:owner_id,created=:created,prove=:prove," +
                "target_audience=:target_audience,series_parent_id=:series_parent_id where id=:id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", obj.ID);
                    cmd.Parameters.AddWithValue(":name", obj.Name);
                    cmd.Parameters.AddWithValue(":description", string.IsNullOrEmpty(obj.Description) ? DBNull.Value : obj.Description);
                    cmd.Parameters.AddWithValue(":points", obj.Points);
                    cmd.Parameters.AddWithValue(":validity_start", obj.ValidityStart);
                    cmd.Parameters.AddWithValue(":validity_end", obj.ValidityEnd);
                    cmd.Parameters.AddWithValue(":category", (int)obj.Category);
                    cmd.Parameters.AddWithValue(":owner_id", obj.OwnerID);
                    cmd.Parameters.AddWithValue(":created", obj.Created);
                    cmd.Parameters.AddWithValue(":prove", (int)obj.Prove);
                    cmd.Parameters.AddWithValue(":target_audience", string.IsNullOrEmpty(obj.TargetAudience) ? DBNull.Value : obj.TargetAudience);
                    cmd.Parameters.AddWithValue(":series_parent_id", obj.SeriesParentID.HasValue ? obj.SeriesParentID.Value : DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Save(Challenge obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Delete(int id)
        {
            string sql = "delete from challenges where id=:id";

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

        public IEnumerable<Challenge> OfBadge(int badge_id)
        {
            string sql = "select id,name,description,points,validity_start," +
                "validity_end,category,owner_id,created,prove,target_audience," +
                "series_parent_id " + 
                "from game_badge_challenges " +
                "inner join challenges on challenge=id where game_badge = :badge_id " +
                "order by created desc";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":badge_id", badge_id);
                    var result = new List<Challenge>();

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

        public IEnumerable<Challenge> NotOfBadge(int badge_id)
        {
            string sql = "select id,name,description,points,validity_start," +
                "validity_end,category,owner_id,created,prove,target_audience," +
                "series_parent_id from challenges where id not in " +
                "(select challenge from game_badge_challenges where game_badge=:badge_id) " +
                "order by created desc";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":badge_id", badge_id);
                    var result = new List<Challenge>();

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

        public IEnumerable<Challenge> AllForUser(int user_id)
        {
            return new List<Challenge>();
        }
    }
}