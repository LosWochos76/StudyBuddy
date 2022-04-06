using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string connection_string;

        public NotificationRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        public IEnumerable<Notification> All(NotificationFilter filter)
        {
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
            var sql = "select id, owner_id, title, body, created, updated from notifications where true";
            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);
            sql += "limit :max offset :from";


            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId.Value);
                sql += " and (owner_id=:owner_id)";
            }

            return qh.ExecuteQueryToObjectList(sql);
        }

        public void Insert(Notification obj)
        {
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
            qh.AddParameter(":owner_id", obj.OwnerId);
            qh.AddParameter(":title", obj.Title);
            qh.AddParameter(":body", obj.Body);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":updated", obj.Updated);

            obj.Id = qh.ExecuteScalar(
                "insert into notifications (owner_id, title, body, created , updated) values (:owner_id, :title, :body, :created , :updated) returning id");
        }

        public IEnumerable<Notification> GetUserNotificationsFeed(NotificationFilter filter)
        {
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
            qh.AddParameter(":user_id", filter.OwnerId);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var sql =
                "select n.id, n.owner_id, n.title, n.body, n.created, n.updated, " +
                "u.firstname, u.lastname, u.nickname, " +
                "md.liked, md.seen, md.shared " +
                "from friends as f " +
                "inner join users as u on f.user_b = :user_id " +
                "inner join (select id, owner_id, title, body, created, updated from notifications limit :max offset :from) as n on u.id = n.owner_id " +
                "left outer join notification_user_metadata  as md on md.owner_id = :user_id and md.notification_id = n.id ";


            return qh.ExecuteQueryToObjectList(sql);
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
            if (!qh.TableExists("notifications"))
                qh.ExecuteNonQuery(
                    "create table notifications (" +
                    "id serial primary key, " +
                    "owner_id int not null, " +
                    "title text, " +
                    "body text, " +
                    "created timestamp default current_timestamp, " +
                    "updated timestamp default current_timestamp " +
                    ")");
        }

        private Notification FromNotificationReader(NpgsqlDataReader reader)
        {
            var obj = new Notification();

            obj.Id = reader.GetInt32(0);
            obj.OwnerId = reader.GetInt32(1);
            obj.Title = reader.GetString(2);
            obj.Body = reader.GetString(3);
            obj.Created = reader.GetDateTime(4);
            obj.Updated = reader.GetDateTime(5);

            var owner = new User();
            owner.Firstname = reader.GetString(6);
            owner.Lastname = reader.GetString(7);
            owner.Nickname = reader.GetString(8);
            obj.Owner = owner;

            obj.Liked = !reader.IsDBNull(9) && reader.GetBoolean(9);
            obj.Shared = !reader.IsDBNull(10) && reader.GetBoolean(10);
            obj.Seen = !reader.IsDBNull(11) && reader.GetBoolean(11);

            return obj;
        }
    }
}