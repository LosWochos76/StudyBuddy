namespace StudyBuddy.Persistence
{
    internal class RevisionHelper
    {
        private readonly string connection_string;
        private readonly string table_name;

        public RevisionHelper(string connection_string, string table_name)
        {
            this.connection_string = connection_string;
            this.table_name = table_name;

            CreateTablesTable();
            Prepare();
        }

        public void CreateTablesTable()
        {
            var qh = new QueryHelper<RevisionHelper>(connection_string);
            if (!qh.TableExists("tables"))
                qh.ExecuteNonQuery("create table tables (" +
                                   "table_name varchar(100) not null, " +
                                   "revision int not null)");
        }

        public void Prepare()
        {
            if (GetRevision() != 0)
                return;

            var qh = new QueryHelper<RevisionHelper>(connection_string);
            qh.AddParameter(":table_name", table_name);
            qh.AddParameter(":revision", 1);
            qh.ExecuteNonQuery("insert into tables (table_name,revision) values (:table_name,:revision)");
        }

        public int GetRevision()
        {
            var qh = new QueryHelper<RevisionHelper>(connection_string);
            qh.AddParameter(":table_name", table_name);
            return qh.ExecuteQueryToSingleInt("SELECT revision FROM tables where table_name=:table_name");
        }

        public void SetRevision(int revision)
        {
            var qh = new QueryHelper<RevisionHelper>(connection_string);
            qh.AddParameters(new {table_name, revision});
            qh.ExecuteNonQuery("update tables set revision=:revision where table_name=:table_name");
        }
    }
}