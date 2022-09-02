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

            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId.Value);
                sql += " and (owner_id=:owner_id) ";
            }

            sql += "limit :max offset :from";
            var set = qh.ExecuteQuery(sql);
            return converter.Multiple(set);
        }

        public int GetCount(NotificationFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            var sql = "select count(*) from notifications where true";

            if (filter.OwnerId.HasValue)
            {
                qh.AddParameter(":owner_id", filter.OwnerId.Value);
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
                "insert into notifications (owner_id, badge_id ,title, body, created , updated) values (:owner_id, :badge_id ,:title, :body, :created , :updated) returning id");
        }

        public Notification ById(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":notification_id", id);

            var set = qh.ExecuteQuery(
                "select id, badge_id , owner_id, title, body, created, updated from  notifications where id = :notification_id");

            return converter.Single(set);
        }

        public IEnumerable<Notification> GetUserNotificationsFeed(NotificationFilter filter)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", filter.OwnerId);
            qh.AddParameter(":from", filter.Start);
            qh.AddParameter(":max", filter.Count);

            var sql =
                "select distinct on (n.id) n.id, n.badge_id ,n.owner_id, n.title, n.body, n.created, n.updated, " +
                "u.firstname, u.lastname, u.nickname, " +
                "md.liked, md.seen, md.shared " +
                "from friends as f " +
                "inner join users as u on f.user_a = :user_id " +
                "inner join (select distinct on (id) id, badge_id ,owner_id, title, body, created, updated from notifications order by id limit :max offset :from) as n on u.id = n.owner_id " +
                "left outer join notification_user_metadata  as md on md.owner_id = :user_id and md.notification_id = n.id " +
                "order by n.id, n.created desc";   

            var set = qh.ExecuteQuery(sql);
            return converter.Multiple(set);
        }

        private void CreateTable()
        {
            var rh = new RevisionHelper(connection_string, "notifications");
            var qh = new QueryHelper(connection_string);

            if (!qh.TableExists("notifications"))
                qh.ExecuteNonQuery(
                    "create table notifications (" +
                    "id serial primary key, " +
                    "badge_id int , " +
                    "owner_id int not null, " +
                    "title text, " +
                    "body text, " +
                    "created timestamp default current_timestamp, " +
                    "updated timestamp default current_timestamp " +
                    ")");

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
            
            if (rh.GetRevision() == 3)
            {
                qh.ExecuteNonQuery("ALTER TABLE notifications ADD FOREIGN KEY (owner_id) REFERENCES users(id);");
                qh.ExecuteNonQuery("ALTER TABLE notifications ADD FOREIGN KEY (badge_id) REFERENCES game_badges(id);");

                rh.SetRevision(4);
            }
            
        }

        public void Delete(int id)
        {
            var qh = new QueryHelper(connection_string);
            qh.Delete("notifications", "id", id);
        }
    }
}