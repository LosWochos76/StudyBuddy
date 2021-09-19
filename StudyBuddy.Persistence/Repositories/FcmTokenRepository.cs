using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class FcmTokenRepository : IFcmTokenRepository
    {
        private readonly string connection_string;

        public FcmTokenRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        public IEnumerable<FcmToken> OfUser(int user_id, int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<FcmToken>(connection_string, FromReader);
            qh.AddParameter(":from", from);
            qh.AddParameter(":max", max);
            qh.AddParameter(":user_id", user_id);
            return qh.ExecuteQueryToObjectList(
                "SELECT id,token,user_id,created,last_seen " +
                "FROM fcm_tokens where user_id=:user_id order by created limit :max offset :from");
        }

        public IEnumerable<FcmToken> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<FcmToken>(connection_string, FromReader, new {from, max});
            return qh.ExecuteQueryToObjectList(
                "SELECT id,token,user_id,created,last_seen " +
                "FROM fcm_tokens order by created limit :max offset :from");
        }

        public FcmToken Save(FcmToken obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
            return obj;
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<FcmToken>(connection_string);

            if (!qh.TableExists("fcm_tokens"))
                qh.ExecuteNonQuery(
                    "create table fcm_tokens (" +
                    "id serial primary key, " +
                    "token text not null, " +
                    "user_id int not null, " +
                    "created date not null, " +
                    "last_seen date not null )");
        }

        public void Insert(FcmToken obj)
        {
            var qh = new QueryHelper<FcmToken>(connection_string, FromReader);
            qh.AddParameter(":user_id", obj.UserID);
            qh.AddParameter(":token", obj.Token);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":last_seen", obj.LastSeen);

            obj.ID = qh.ExecuteScalar(
                "insert into fcm_tokens (user_id,token,created,last_seen) values (:user_id,:token,:created,:last_seen) RETURNING id");
        }

        public void Update(FcmToken obj)
        {
            var qh = new QueryHelper<FcmToken>(connection_string, FromReader);
            obj.LastSeen = DateTime.Now.Date;

            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":token", obj.Token);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":last_seen", obj.LastSeen);

            qh.ExecuteNonQuery("update fcm_tokens set token=:token,created=:created,last_seen=:last_seen where id=:id");
        }

        private FcmToken FromReader(NpgsqlDataReader reader)
        {
            var obj = new FcmToken();
            obj.ID = reader.GetInt32(0);
            obj.Token = reader.GetString(1);
            obj.UserID = reader.GetInt32(2);
            obj.Created = reader.GetDateTime(3);
            obj.LastSeen = reader.GetDateTime(4);

            return obj;
        }
    }
}