using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class BusinessEventRepository : IBusinessEventRepository
    {
        private readonly string connection_string;

        public BusinessEventRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<BusinessEvent>(connection_string, FromReader);
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
            var qh = new QueryHelper<BusinessEvent>(connection_string, FromReader);
            qh.AddParameter(":id", id);
            return qh.ExecuteQueryToSingleObject("select id,name,owner_id,type,code from business_events where id=:id");
        }

        public IEnumerable<BusinessEvent> All(BusinessEventFilter filter)
        {
            var qh = new QueryHelper<BusinessEvent>(connection_string, FromReader);
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);
            var sql = "select id,name,owner_id,type,code from business_events limit :max offset :from";
            return qh.ExecuteQueryToObjectList(sql);
        }

        public BusinessEvent Insert(BusinessEvent obj)
        {
            var qh = new QueryHelper<BusinessEvent>(connection_string, FromReader);
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
            var qh = new QueryHelper<BusinessEvent>(connection_string, FromReader);
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
            var qh = new QueryHelper<BusinessEvent>(connection_string, FromReader);
            qh.Delete("business_events", "id", id);
        }

        private BusinessEvent FromReader(NpgsqlDataReader reader)
        {
            var obj = new BusinessEvent();
            obj.ID = reader.GetInt32(0);
            obj.Name = reader.GetString(1);
            obj.OwnerID = reader.GetInt32(2);
            obj.Type = (BusinessEventType)reader.GetInt32(3);
            obj.Code = reader.GetString(4);
            return obj;
        }
    }
}
