using System;
using System.Collections.Generic;
using Npgsql;

namespace StudyBuddy.Persistence
{
    public delegate T ObjectReader<T>(NpgsqlDataReader reader);

    public class QueryHelper<T> where T : class
    {
        private string connection_string = "";
        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        public QueryHelper(string connection_string)
        {
            this.connection_string = connection_string;
        }

        public void AddParameter(string name, object value)
        {
            parameters.Add(name, value);
        }

        public T ExecuteQueryToSingleObject(string sql, ObjectReader<T> object_reader)
        {
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
                            return object_reader(reader);
                        }
                    }
                }
            }

            return null;
        }

        public IEnumerable<T> ExecuteQueryToObjectList(string sql, ObjectReader<T> object_reader)
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
        }

        public int ExecuteScalar(string sql)
        {
            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    foreach (var param in this.parameters)
                        cmd.Parameters.AddWithValue(param.Key, param.Value);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int GetCount(string table_name)
        {
            string sql = "SELECT count(*) as count FROM " + table_name;
            int result = 0;

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = reader.GetInt32(0);
                    }
                }
            }

            return result;
        }

        public void Delete(string table_name, string field_name, int id)
        {
            AddParameter(":id", id);
            ExecuteNonQuery(string.Format("delete from {0} where {1}=:id", table_name, field_name));
        }
    }
}
