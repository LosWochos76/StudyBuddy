using System.Collections.Generic;
using System.Text;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class BusinessEventRepository : IBusinessEventRepository
    {
        private readonly string connection_string;
        private readonly BusinessEventConverter converter = new BusinessEventConverter();

        public BusinessEventRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {
            var qh = new QueryHelper(connection_string);
            if (!qh.TableExists("business_events"))
            {
                qh.ExecuteNonQuery(
                    "create table business_events (" +
                    "id serial primary key, " +
                    "name varchar(100) not null, " +
                    "owner_id int not null, " +
                    "type smallint not null, " +
                    "code text)");
            }
        }

        public BusinessEvent GetById(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":id", id);
            var set = qh.ExecuteQuery("select id,name,owner_id,type,code from business_events where id=:id");
            return converter.Single(set);
        }

        public IEnumerable<BusinessEvent> All(BusinessEventFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);
            var sql = new StringBuilder("select id,name,owner_id,type,code from business_events ");

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql.Append("where name ilike :search_text ");
            }

            sql.Append("order by id,name limit :max offset :from");
            var set = qh.ExecuteQuery(sql.ToString());
            return converter.Multiple(set);
        }

        public int GetCount(BusinessEventFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            var sql = "select count(*) from business_events ";

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                qh.AddParameter(":search_text", "%" + filter.SearchText + "%");
                sql += "where name ilike :search_text";
            }

            return qh.ExecuteQueryToSingleInt(sql);
        }

        public BusinessEvent Insert(BusinessEvent obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":type", (int)obj.Type);
            qh.AddParameter(":code", obj.Code);

            obj.ID = qh.ExecuteScalar(
                "insert into business_events (name,owner_id,type,code) values " +
                "(:name,:owner_id,:type,:code) RETURNING id");

            return obj;
        }

        public BusinessEvent Update(BusinessEvent obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":name", obj.Name);
            qh.AddParameter(":owner_id", obj.OwnerID);
            qh.AddParameter(":type", (int)obj.Type);
            qh.AddParameter(":code", obj.Code);
            qh.ExecuteNonQuery("update business_events set name=:name,owner_id=:owner_id,type=:type,code=:code where id=:id");
            return obj;
        }

        public BusinessEvent Save(BusinessEvent obj)
        {
            if (obj.ID == 0)
                return Insert(obj);
            else
                return Update(obj);
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.Delete("business_events", "id", id);
        }
    }
}
