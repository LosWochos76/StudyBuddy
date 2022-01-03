using System;
using System.Collections.Generic;
using Npgsql;

namespace StudyBuddy.Persistence
{
    internal delegate T ObjectReader<T>(NpgsqlDataReader reader);

    internal class QueryHelper<T> where T : class
    {
        private readonly string connection_string;
        private readonly ObjectReader<T> object_reader;
        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

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
                    foreach (var param in parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = object_reader(reader);
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
                    foreach (var param in parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    using (var reader = cmd.ExecuteReader())
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
                    foreach (var param in parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    cmd.ExecuteNonQuery();
                }
            }

            parameters.Clear();
        }

        public int ExecuteQueryToSingleInt(string sql)
        {
            var result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    using (var reader = cmd.ExecuteReader())
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
            var result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in parameters)
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
            var result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT count(*) as count FROM " + table_name, connection))
                {
                    using (var reader = cmd.ExecuteReader())
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
    }
}