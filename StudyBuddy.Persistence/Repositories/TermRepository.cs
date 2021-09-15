using System;
using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class TermRepository : ITermRepository
    {
        private string connection_string;

        public TermRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTeamTable();
        }

        private void CreateTeamTable()
        {
            var qh = new QueryHelper<Term>(connection_string, FromReader);

            if (!qh.TableExists("terms"))
            {
                qh.ExecuteNonQuery(
                    "create table terms (" +
                    "id serial primary key, " +
                    "shortname varchar(20) not null," +
                    "name varchar(100)," +
                    "start_date date not null," +
                    "end_date date not null)");
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
            var qh = new QueryHelper<Term>(connection_string, FromReader);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,shortname,name,start_date,end_date FROM terms where id=:id");
        }

        public Term ByDate(DateTime date)
        {
            var qh = new QueryHelper<Term>(connection_string, FromReader);
            qh.AddParameter(":date", date.Date);
            return qh.ExecuteQueryToSingleObject(
                "SELECT id,shortname,name,start_date," +
                "end_date FROM terms where :date>=start_date and :date<=end_date");
        }

        public IEnumerable<Term> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Term>(connection_string, FromReader);
            qh.AddParameters(new { from, max });
            return qh.ExecuteQueryToObjectList(
                "SELECT id,shortname,name,start_date,end_date " +
                "FROM terms order by start_date limit :max offset :from");
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<Term>(connection_string, FromReader);
            qh.Delete("terms", "id", id);
        }

        public void Insert(Term obj)
        {
            var qh = new QueryHelper<Term>(connection_string, FromReader);

            qh.AddParameters(new {
                shortname = obj.ShortName,
                name = obj.Name,
                start_date = obj.Start.Date,
                end_date = obj.End.Date });

            obj.ID = qh.ExecuteScalar(
                "insert into terms " +
                "(shortname,name,start_date,end_date) values " +
                "(:shortname,:name,:start_date,:end_date) RETURNING id");
        }

        public void Update(Term obj)
        {
            var qh = new QueryHelper<Term>(connection_string, FromReader);

            qh.AddParameters(new
            {
                id = obj.ID,
                shortname = obj.ShortName,
                name = obj.Name,
                start_date = obj.Start.Date,
                end_date = obj.End.Date
            });

            qh.ExecuteNonQuery(
                "update terms set shortname=:shortname," +
                "name=:name,start_date=:start_date,end_date=:end_date where id=:id");
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
            return ByDate(DateTime.Now.Date);
        }
    }
}