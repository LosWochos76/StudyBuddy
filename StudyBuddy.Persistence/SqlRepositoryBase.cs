using System;
using Npgsql;

namespace StudyBuddy.Persistence
{
    public abstract class SqlRepositoryBase
    {
        protected string connection_string;

        public SqlRepositoryBase(string connection_string)
        {
            this.connection_string = connection_string;
        }

        protected bool TableExists(string table_name) 
        {
            var sql = "SELECT EXISTS (SELECT FROM pg_tables WHERE tablename=:table_name)";

            using (var connection = new NpgsqlConnection(connection_string))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue(":table_name", table_name);
                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
        }
    }
}