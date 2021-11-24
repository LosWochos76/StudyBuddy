using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;
using StudyBuddy.Model.Model;

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

        private void CreateTable()
        {
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
            if (!qh.TableExists("notifications"))
            {
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
        }

        public IEnumerable<Notification> All(NotificationFilter filter)
        {
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
            var sql = "select id, owner_id, title, body, created, updated from notifications where true";

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

        public IEnumerable<UserNotification> GetUserNotificationsFeed(NotificationFilter filter)
        {
            var qh = new QueryHelper<UserNotification>(connection_string, FromUserNotificationReader);
            qh.AddParameter(":user_id", filter.OwnerId);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            return qh.ExecuteQueryToObjectList(
                "select notifications.id, notifications.owner_id, notifications.title, notifications.body, notifications.created, notifications.updated, users.firstname, users.lastname, users.nickname " +
                "from friends " +
                "inner join users on friends.user_b = :user_id " +
                "inner join notifications on users.id = notifications.owner_id ");
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

            return obj;
        }

        private UserNotification FromUserNotificationReader(NpgsqlDataReader reader)
        {
            var obj = new UserNotification();

            obj.Id = reader.GetInt32(0);
            obj.OwnerId = reader.GetInt32(1);
            obj.Title = reader.GetString(2);
            obj.Body = reader.GetString(3);
            obj.Created = reader.GetDateTime(4);
            obj.Updated = reader.GetDateTime(5);
            obj.Firstname = reader.GetString(6);
            obj.Lastname = reader.GetString(7);
            obj.Nickname = reader.GetString(8);

            return obj;
        }
    }
}