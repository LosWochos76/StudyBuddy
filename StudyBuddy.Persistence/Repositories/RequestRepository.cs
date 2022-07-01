using System.Collections.Generic;
using System.Text;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    internal class RequestRepository : IRequestRepository
    {
        private readonly string connection_string;
        private readonly RequestConverter converter = new RequestConverter();

        public RequestRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        public IEnumerable<Request> All(RequestFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var sql = new StringBuilder("select id,created,sender_id,recipient_id,type,challenge_id from requests where true ");

            if (filter.OnlyForSender.HasValue)
            {
                qh.AddParameter(":sender_id", filter.OnlyForSender.Value);
                sql.Append(" and (sender_id=:sender_id)");
            }

            if (filter.OnlyForRecipient.HasValue)
            {
                qh.AddParameter(":recipient_id", filter.OnlyForRecipient.Value);
                sql.Append(" and (recipient_id=:recipient_id)");
            }

            if (filter.OnlyForType.HasValue)
            {
                qh.AddParameter(":type", (int)filter.OnlyForType.Value);
                sql.Append(" and (type=:type)");
            }

            sql.Append(" order by created desc limit :max offset :from");
            var set = qh.ExecuteQuery(sql.ToString());
            return converter.Multiple(set);
        }

        public int GetCount(RequestFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            var sql = new StringBuilder("select count(*) from requests where true ");

            if (filter.OnlyForSender.HasValue)
            {
                qh.AddParameter(":sender_id", filter.OnlyForSender.Value);
                sql.Append(" and (sender_id=:sender_id)");
            }

            if (filter.OnlyForRecipient.HasValue)
            {
                qh.AddParameter(":recipient_id", filter.OnlyForRecipient.Value);
                sql.Append(" and (recipient_id=:recipient_id)");
            }

            if (filter.OnlyForType.HasValue)
            {
                qh.AddParameter(":type", (int)filter.OnlyForType.Value);
                sql.Append(" and (type=:type)");
            }

            return qh.ExecuteQueryToSingleInt(sql.ToString());
        }

        public Request ById(int id)
        {
            var qh = new QueryHelper(connection_string, new {id});
            var set = qh.ExecuteQuery(
                "select id,created,sender_id,recipient_id,type,challenge_id " +
                "from requests where id=:id");

            return converter.Single(set);
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.Delete("requests", "id", id);
        }

        public void Insert(Request obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":sender_id", obj.SenderID);
            qh.AddParameter(":recipient_id", obj.RecipientID);
            qh.AddParameter(":type", (int) obj.Type);
            qh.AddParameter(":challenge_id", obj.ChallengeID);
            obj.ID = qh.ExecuteScalar(
                "insert into requests (created,sender_id,recipient_id,type,challenge_id) values " +
                "(:created,:sender_id,:recipient_id,:type,:challenge_id) returning id");
        }

        private void CreateTable()
        {
            var rh = new RevisionHelper(connection_string, "requests");
            var qh = new QueryHelper(connection_string);

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

                rh.SetRevision(2);
            }

            if (rh.GetRevision() == 1)
            {
                qh.ExecuteNonQuery(
                    "alter table requests " +
                    "add column created date;");

                rh.SetRevision(2);
            }
        }

        public Request FindFriendshipRequest(int sender_id, int recipient_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":sender_id", sender_id);
            qh.AddParameter(":recipient_id", recipient_id);
            qh.AddParameter(":type", (int)RequestType.Friendship);

            var set = qh.ExecuteQuery(
                "select id,created,sender_id,recipient_id,type,challenge_id " +
                "from requests where sender_id=:sender_id and recipient_id=:recipient_id and type=:type and challenge_id is null");

            return converter.Single(set);
        }

        public Request FindSimilar(Request obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":sender_id", obj.SenderID);
            qh.AddParameter(":recipient_id", obj.RecipientID);
            qh.AddParameter(":type", (int)obj.Type);

            var sql = new StringBuilder("select id,created,sender_id,recipient_id,type,challenge_id " +
                    "from requests where sender_id=:sender_id and recipient_id=:recipient_id and type=:type");

            if (obj.Type == RequestType.ChallengeAcceptance)
            {
                qh.AddParameter(":challenge_id", obj.ChallengeID);
                sql.Append(" and challenge_id=:challenge_id");
            }

            var set = qh.ExecuteQuery(sql.ToString());
            return converter.Single(set);
        }
    }
}