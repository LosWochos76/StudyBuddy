using Npgsql;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;

namespace StudyBuddy.Persistence
{
    class ChallengeRepository : SqlRepositoryBase, IChallengeRepository
    {
        public ChallengeRepository(NpgsqlConnection connection) : base(connection)
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
                "target_date date not null, " +
                "category smallint not null, " +
                "owner_id int not null, " + 
                "created date not null, " +
                "prove smallint not null, " + 
                "target_audience text)";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.ExecuteNonQuery();
            }
        }

        private Challenge FromReader(NpgsqlDataReader reader)
        {
            var obj = new Challenge();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.Description = reader.IsDBNull(2) ? null : reader.GetString(2);
            obj.Points = reader.GetInt32(3);
            obj.TargetDate = reader.GetDateTime(4);
            obj.Category = (ChallengeCategory)reader.GetInt32(5);
            obj.OwnerID = reader.GetInt32(6);
            obj.Created = reader.GetDateTime(7);
            obj.Prove = (ChallengeProve)reader.GetInt32(8);
            obj.TargetAudience = reader.IsDBNull(9) ? null : reader.GetString(9);
            return obj;
        }

        public Challenge ById(int id)
        {
            string sql = "SELECT id,name,description,points," + 
                "target_date,category,owner_id,created,prove,target_audience FROM challenges where id=:id";

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

            return null;
        }

        public IEnumerable<Challenge> All(int from = 0, int max = 1000)
        {
            string sql = "SELECT id,name,description,points,target_date,category,owner_id," + 
                "created,prove,target_audience FROM challenges order by created limit :max offset :from";
            
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

        private void Insert(Challenge obj)
        {
            string sql = "insert into challenges " +
                    "(name,description,points,target_date,category,owner_id,created,prove,target_audience) values " +
                    "(:name,:description,:points,:target_date,:category,:owner_id,:created,:prove,:target_audience) RETURNING id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":name", obj.Name);
                cmd.Parameters.AddWithValue(":description", string.IsNullOrEmpty(obj.Description) ? DBNull.Value : obj.Description);
                cmd.Parameters.AddWithValue(":points", obj.Points);
                cmd.Parameters.AddWithValue(":target_date", obj.TargetDate);
                cmd.Parameters.AddWithValue(":category", (int)obj.Category);
                cmd.Parameters.AddWithValue(":owner_id", obj.OwnerID);
                cmd.Parameters.AddWithValue(":created", obj.Created);
                cmd.Parameters.AddWithValue(":prove", (int)obj.Prove);
                cmd.Parameters.AddWithValue(":target_audience", string.IsNullOrEmpty(obj.TargetAudience) ? DBNull.Value : obj.TargetAudience);
                obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void Update(Challenge obj)
        {
            string sql = "update challenges set name=:name,description=:description,points=:points," + 
                "target_date=:target_date,category=:category,owner_id=:owner_id,created=:created,prove=:prove," +
                "target_audience=:target_audience where id=:id";

            using (var cmd = new NpgsqlCommand(sql, connection)) 
            {
                cmd.Parameters.AddWithValue(":id", obj.ID);
                cmd.Parameters.AddWithValue(":name", obj.Name);
                cmd.Parameters.AddWithValue(":description", string.IsNullOrEmpty(obj.Description) ? DBNull.Value : obj.Description);
                cmd.Parameters.AddWithValue(":points", obj.Points);
                cmd.Parameters.AddWithValue(":target_date", obj.TargetDate);
                cmd.Parameters.AddWithValue(":category", (int)obj.Category);
                cmd.Parameters.AddWithValue(":owner_id", obj.OwnerID);
                cmd.Parameters.AddWithValue(":created", obj.Created);
                cmd.Parameters.AddWithValue(":prove", (int)obj.Prove);
                cmd.Parameters.AddWithValue(":target_audience", string.IsNullOrEmpty(obj.TargetAudience) ? DBNull.Value : obj.TargetAudience);
                cmd.ExecuteNonQuery();
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
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue(":id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}