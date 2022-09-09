using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence.Test
{
    public class BaseTest
    {
        protected string db_name;
        protected Repository repository;

        protected void Create()
        {
            this.db_name = RandomDbName();
            var qh = new QueryHelper(GetMainConnectionString());
            qh.ExecuteNonQuery("create database " + db_name);
            this.repository = new Repository(GetTestConnectionString());
        }

        protected void Drop()
        {
            var qh = new QueryHelper(GetMainConnectionString());
            qh.ExecuteNonQuery("SELECT pid, pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname='" + db_name + "'");
            qh.ExecuteNonQuery("drop database if exists " + db_name);
        }

        private string RandomDbName()
        {
            const string allowed_chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            Random rnd = new Random();
            char[] chars = new char[10];

            for (int i = 0; i < 10; i++)
                chars[i] = allowed_chars[rnd.Next(0, allowed_chars.Length)];

            return "test_" + new string(chars);
        }

        protected string GetMainConnectionString()
        {
            return string.Format("Host=localhost;Username=postgres;Password=secret;Database=postgres");
        }

        protected string GetTestConnectionString()
        {
            return string.Format("Host=localhost;Username=postgres;Password=secret;Database=" + db_name);
        }
    }
}