using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string connection_string;
        private readonly NotificationConverter converter = new NotificationConverter();

        public NotificationRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        public IEnumerable<Notification> GetAll(NotificationFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            var sql = "select id, badge_id, owner_id, title, body, created, updated from notifications where true ";

            qh.AddParameter(":max", filter.Count);
            qh.AddParameter(":from", filter.Start);

            if (filter.UserID.HasValue)
            {
                qh.AddParameter(":owner_id", filter.UserID.Value);
                sql += " and (owner_id=:owner_id) ";
            }
            if(filter.OrderAscending)
            {
                sql += "order by created asc ";
            }
            else if (!filter.OrderAscending)
            {
                sql += "order by created desc ";
            }
            
            sql += "limit :max offset :from";
            var set = qh.ExecuteQuery(sql);
            return converter.Multiple(set);
        }

        public int GetCount(NotificationFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            var sql = "select count(*) from notifications where true";

            if (filter.UserID.HasValue)
            {
                qh.AddParameter(":owner_id", filter.UserID.Value);
                sql += " and (owner_id=:owner_id)";
            }

            return qh.ExecuteQueryToSingleInt(sql);
        }

        public void Insert(Notification obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":owner_id", obj.OwnerId);
            qh.AddParameter(":badge_id", obj.BadgeId.HasValue ? obj.BadgeId : DBNull.Value);
            qh.AddParameter(":title", obj.Title);
            qh.AddParameter(":body", obj.Body);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":updated", obj.Updated);

            obj.Id = qh.ExecuteScalar(
                "insert into notifications (owner_id, badge_id ,title, body, created, updated) " +
                "values (:owner_id, :badge_id ,:title, :body, :created, :updated) returning id");
        }

        public Notification ById(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":notification_id", id);

            var set = qh.ExecuteQuery(
                "select id, badge_id, owner_id, title, body, created, updated from notifications where id = :notification_id");

            return converter.Single(set);
        }

        public IEnumerable<Notification> GetNotificationsForFriends(NotificationFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":owner_id", filter.UserID);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var sql = "select id,owner_id,body,created,updated,badge_id from notifications " +
                "inner join friends on owner_id = user_a " +
                "where user_b = :owner_id " +
                "order by created desc " +
                "limit :max offset :from";

            var set = qh.ExecuteQuery(sql);
            return converter.Multiple(set);
        }

        private void CreateTable()
        {
            var rh = new RevisionHelper(connection_string, "notifications");
            var qh = new QueryHelper(connection_string);

            if (!qh.TableExists("notifications"))
            {
                qh.ExecuteNonQuery(
                    "create table notifications (" +
                    "id serial primary key, " +
                    "badge_id int, " +
                    "owner_id int not null, " +
                    "title text, " +
                    "body text, " +
                    "created date, " +
                    "updated date " +
                    ")");

                rh.SetRevision(3);
            }

            if (rh.GetRevision() == 1)
            {
                qh.ExecuteNonQuery(
                    "alter table notifications " +
                    "add column badge_id int");
                
                rh.SetRevision(2);
            }
            
            if (rh.GetRevision() == 2)
            {
                qh.ExecuteNonQuery("ALTER TABLE notifications ALTER COLUMN created type date");
                qh.ExecuteNonQuery("ALTER TABLE notifications ALTER COLUMN updated type date");
                rh.SetRevision(3);
            }
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.Delete("notifications", "id", id);
        }
    }
}