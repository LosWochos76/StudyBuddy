using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class FcmTokenRepository : IFcmTokenRepository
    {
        private readonly string connection_string;
        private readonly FcmTokenConverter converter = new FcmTokenConverter();

        public FcmTokenRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        public IEnumerable<FcmToken> GetAll(FcmTokenFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var set = qh.ExecuteQuery(
                "SELECT id,token,user_id,created,last_seen " +
                "FROM fcm_tokens order by created limit :max offset :from");

            return converter.Multiple(set);
        }

        public IEnumerable<FcmToken> GetForUser(int user_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", user_id);

            var set = qh.ExecuteQuery(
                "SELECT id,token,user_id,created,last_seen " +
                "FROM fcm_tokens order by created where user_id=:user_id");

            return converter.Multiple(set);
        }

        public FcmToken Save(FcmToken obj)
        {
            var existingToken = GetByToken(obj.Token);

            if (existingToken is null)
                Insert(obj);
            else
                Update(existingToken);

            return obj;
        }

        private void CreateTable()
        {
            var qh = new QueryHelper(connection_string);

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
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", obj.UserID);
            qh.AddParameter(":token", obj.Token);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":last_seen", obj.LastSeen);
            obj.ID = qh.ExecuteScalar(
                "insert into fcm_tokens (user_id,token,created,last_seen) values (:user_id,:token,:created,:last_seen) RETURNING id");
        }

        public void DeleteOldTokens()
        {
            var qh = new QueryHelper(connection_string);
            qh.ExecuteScalar(
                "delete from fcm_tokens WHERE last_seen < NOW() - INTERVAL '60 days'");
        }

        public void Update(FcmToken obj)
        {
            var qh = new QueryHelper(connection_string);
            obj.LastSeen = DateTime.Now.Date;
            qh.AddParameter(":id", obj.ID);
            qh.AddParameter(":token", obj.Token);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":last_seen", obj.LastSeen);
            qh.ExecuteNonQuery("update fcm_tokens set token=:token,created=:created,last_seen=:last_seen where id=:id");
        }

        public FcmToken GetByToken(string token)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":token", token);
            var set = qh.ExecuteQuery("select * from fcm_tokens where token=:token");
            return converter.Single(set);
        }

        public int GetCount(FcmTokenFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            return qh.ExecuteQueryToSingleInt("select count(*) from fcm_tokens");
        }
    }
}