using Npgsql;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace StudyBuddy.Persistence
{
    class RequestRepository : IRequestRepository
    {
        private string connection_string;

        public RequestRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CreateTable() 
        {
            var qh = new QueryHelper<Request>(connection_string, FromReader);

            if (!qh.TableExists("requests"))
            {
                qh.ExecuteNonQuery(
                    "create table requests (" +
                    "id serial primary key, " +
                    "created date not null, " +
                    "sender_id int not null, " +
                    "recipient_id int not null, " +
                    "type int not null, " +
                    "challenge_id int)");

                    qh.SetRevision("requests", 2);
            }

            int revision = qh.GetRevision("requests");
            if (revision == 1)
            {
                qh.ExecuteNonQuery(
                    "alter table requests " +
                    "add column created date;");

                qh.SetRevision("requests", 2);
            }
        }

        private Request FromReader(NpgsqlDataReader reader)
        {
            var obj = new Request();
            obj.ID = reader.GetInt32(0);
            obj.Created = reader.GetDateTime(1);
            obj.SenderID = reader.GetInt32(2);
            obj.RecipientID = reader.GetInt32(3);
            obj.Type = (RequestType)reader.GetInt32(4);
            obj.ChallengeID = reader.IsDBNull(5) ? null : reader.GetInt32(5);
            return obj;
        }

        public IEnumerable<Request> All(int from = 0, int max = 1000)
        {
            var qh = new QueryHelper<Request>(connection_string, FromReader, new { from, max });
            return qh.ExecuteQueryToObjectList(
                "select id,created,sender_id,recipient_id,type,challenge_id " +
                "from requests order by created desc limit :max offset :from");
        }

        public Request ById(int id)
        {
            var qh = new QueryHelper<Request>(connection_string, FromReader, new { id });
            return qh.ExecuteQueryToSingleObject(
                "select id,created,sender_id,recipient_id,type,challenge_id " +
                "from requests where id=:id");
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper<Request>(connection_string);
            qh.Delete("requests", "id", id);
        }

        public IEnumerable<Request> OfSender(int sender_id)
        {
            var qh = new QueryHelper<Request>(connection_string, FromReader, new { sender_id });
            return qh.ExecuteQueryToObjectList(
                "select id,created,sender_id,recipient_id,type,challenge_id " +
                "from requests where sender_id=:sender_id order by created desc");
        }

        public IEnumerable<Request> ForRecipient(int recipient_id)
        {
            var qh = new QueryHelper<Request>(connection_string, FromReader, new { recipient_id });
            return qh.ExecuteQueryToObjectList(
                "select id,created,sender_id,recipient_id,type,challenge_id " +
                "from requests where recipient_id=:recipient_id order by created desc");
        }

        public void Insert(Request obj)
        {
            var qh = new QueryHelper<Request>(connection_string);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":sender_id", obj.SenderID);
            qh.AddParameter(":recipient_id", obj.RecipientID);
            qh.AddParameter(":type", (int)obj.Type);
            qh.AddParameter(":challenge_id", obj.ChallengeID.HasValue ? obj.ChallengeID.Value : DBNull.Value);
            obj.ID = qh.ExecuteScalar(
                "insert into requests (created,sender_id,recipient_id,type,challenge_id) values " +
                "(:created,:sender_id,:recipient_id,:type,:challenge_id) returning id");
        }
    }
}