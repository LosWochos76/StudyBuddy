using System;
using System.Collections.Generic;
using Npgsql;

namespace StudyBuddy.Persistence
{
    public delegate T ObjectReader<T>(NpgsqlDataReader reader);

    public class QueryHelper<T> where T : class
    {
        private string connection_string;
        private ObjectReader<T> object_reader;
        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        public QueryHelper(string connection_string)
        {
            this.connection_string = connection_string;
        }

        public QueryHelper(string connection_string, ObjectReader<T> object_reader)
        {
            this.connection_string = connection_string;
            this.object_reader = object_reader;
        }

        public QueryHelper(string connection_string, object parameters)
        {
            this.connection_string = connection_string;
            AddParameters(parameters);
        }

        public QueryHelper(string connection_string, ObjectReader<T> object_reader, object parameters)
        {
            this.connection_string = connection_string;
            this.object_reader = object_reader;
            AddParameters(parameters);
        }

        public void AddParameter(string name, object value)
        {
            parameters.Add(name, value);
        }

        public void AddParameters(object obj)
        {
            foreach (var p in obj.GetType().GetProperties())
                parameters.Add(":" + p.Name, p.GetValue(obj, null));
        }

        public T ExecuteQueryToSingleObject(string sql)
        {
            T result = null;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in this.parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = object_reader(reader);
                        }
                    }
                }
            }

            parameters.Clear();
            return result;
        }

        public IEnumerable<T> ExecuteQueryToObjectList(string sql)
        {
            var result = new List<T>();

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in this.parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var obj = object_reader(reader);
                            result.Add(obj);
                        }
                    }
                }
            }

            parameters.Clear();
            return result;
        }

        public void ExecuteNonQuery(string sql)
        {
            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in this.parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    cmd.ExecuteNonQuery();
                }
            }

            parameters.Clear();
        }

        public int ExecuteQueryToSingleInt(string sql)
        {
            int result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in this.parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = reader.GetInt32(0);
                    }
                }
            }

            parameters.Clear();
            return result;
        }

        public int ExecuteScalar(string sql)
        {
            int result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in this.parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            parameters.Clear();
            return result;
        }

        public void Delete(string table_name, string field_name, int id)
        {
            AddParameter(":id", id);
            ExecuteNonQuery(string.Format("delete from {0} where {1}=:id", table_name, field_name));
        }

        public int GetCount(string table_name)
        {
            int result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT count(*) as count FROM " + table_name, connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = reader.GetInt32(0);
                    }
                }
            }

            parameters.Clear();
            return result;
        }

        public bool TableExists(string table_name)
        {
            var result = false;
            var sql = "SELECT EXISTS (SELECT FROM pg_tables WHERE tablename=:table_name)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":table_name", table_name);
                    result = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            parameters.Clear();
            return result;
        }

        public void CreateTablesTable()
        {
            if (!TableExists("tables"))
                ExecuteNonQuery("create table tables (" +
                    "table_name varchar(100) not null, " +
                    "revision int not null)");
        }

        public int GetRevision(string table_name)
        {
            AddParameters(new { table_name });
            var result = ExecuteQueryToSingleInt("SELECT revision FROM tables where table_name=:table_name");

            if (result == 0)
            {
                AddParameters(new { table_name, revision = 1});
                ExecuteNonQuery("insert into tables (table_name,revision) values (:table_name,:revision)");
                result = 1;
            }

            return result;
        }

        public void SetRevision(string table_name, int revision)
        {
            AddParameters(new { table_name, revision });
            ExecuteNonQuery("update tables set revision=:revision where table_name=:table_name");
        }
    }
}
