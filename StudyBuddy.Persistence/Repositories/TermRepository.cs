using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class TermRepository : SqlRepositoryBase, ITermRepository
    {
        public TermRepository(string connection_string) : base(connection_string)
        {
            if (!TableExists("terms")) 
                CreateTeamTable();
        }

        private void CreateTeamTable() 
        {
            string sql = "create table terms (" +
                "id serial primary key, " +
                "shortname varchar(20) not null," +
                "name varchar(100)," + 
                "start_date date not null," +
                "end_date date not null)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private Term FromReader(NpgsqlDataReader reader)
        {
            var obj = new Term();
            obj.ID = reader.GetInt32(0);
            obj.ShortName = reader.GetString(1);
            obj.Name = reader.GetString(2);
            obj.Start = reader.GetDateTime(3);
            obj.End = reader.GetDateTime(4);
            return obj;
        }

        public Term ById(int id)
        {
            string sql = "SELECT id,shortname,name,start_date," +
                "end_date FROM terms where id=:id";

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

        public Term ByDate(DateTime date)
        {
            string sql = "SELECT id,shortname,name,start_date," +
                "end_date FROM terms where :date>=start_date and :date<=end_date";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":date", date.Date);

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

        public IEnumerable<Term> All(int from = 0, int max = 1000)
        {
            string sql = "SELECT id,shortname,name,start_date,end_date " +
                "FROM terms order by start_date limit :max offset :from";
            var result = new List<Term>();

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.Parameters.AddWithValue(":from", from);
                cmd.Parameters.AddWithValue(":max", max);
                

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = FromReader(reader);
                        result.Add(obj);
                    }
                }
            }
            
            return result;
        }

        public void Delete(int id)
        {
            string sql = "delete from terms where id=:id";

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

        public void Insert(Term obj)
        {
            string sql = "insert into terms " +
                    "(shortname,name,start_date,end_date) values " +
                    "(:shortname,:name,:start_date,:end_date) RETURNING id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":shortname", obj.ShortName);
                    cmd.Parameters.AddWithValue(":name", obj.Name);
                    cmd.Parameters.AddWithValue(":start_date", obj.Start.Date);
                    cmd.Parameters.AddWithValue(":end_date", obj.End.Date);
                    obj.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void Update(Term obj)
        {
            string sql = "update terms set shortname=:shortname," +
                "name=:name,start_date=:start_date,end_date=:end_date where id=:id";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":id", obj.ID);
                    cmd.Parameters.AddWithValue(":shortname", obj.ShortName);
                    cmd.Parameters.AddWithValue(":name", obj.Name);
                    cmd.Parameters.AddWithValue(":start_date", obj.Start.Date);
                    cmd.Parameters.AddWithValue(":end_date", obj.End.Date);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Save(Term obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public Term Current()
        {
            return null;
        }
    }
}