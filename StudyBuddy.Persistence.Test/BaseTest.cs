using StudyBuddy.Model;

namespace StudyBuddy.Persistence.Test
{
    public class BaseTest
    {
        protected Repository repository;

        protected void Create()
        {
            var qh = new QueryHelper(GetMainConnectionString());
            qh.ExecuteNonQuery("SELECT pid, pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname='test'");
            qh.ExecuteNonQuery("drop database if exists test;");
            qh.ExecuteNonQuery("create database test;");
            this.repository = new Repository(GetTestConnectionString());
        }

        protected string GetMainConnectionString()
        {
            return string.Format("Host={0};Username={1};Password={2};Database={3}",
                Environment.GetOrDefault("POSTGRESQL_HOST", "localhost"),
                Environment.GetOrDefault("POSTGRESQL_USER", "postgres"),
                Environment.GetOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Environment.GetOrDefault("POSTGRESQL_DATABASE", "postgres"));
        }

        protected string GetTestConnectionString()
        {
            return string.Format("Host={0};Username={1};Password={2};Database={3}",
                Environment.GetOrDefault("POSTGRESQL_HOST", "localhost"),
                Environment.GetOrDefault("POSTGRESQL_USER", "postgres"),
                Environment.GetOrDefault("POSTGRESQL_PASSWORD", "secret"),
                Environment.GetOrDefault("POSTGRESQL_DATABASE", "test"));
        }
    }
}