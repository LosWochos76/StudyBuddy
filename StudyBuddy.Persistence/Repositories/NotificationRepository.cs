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
            var sql = "select id, badge_id, owner_id, title, body, created, updated from notifications where true";
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
            qh.AddParameter(":badge_id", obj.BadgeId);
            qh.AddParameter(":title", obj.Title);
            qh.AddParameter(":body", obj.Body);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":updated", obj.Updated);

            obj.Id = qh.ExecuteScalar(
                "insert into notifications (owner_id, badge_id ,title, body, created , updated) values (:owner_id, :badge_id ,:title, :body, :created , :updated) returning id");
        }

        public Notification GetNotificationById(int notificationId)
        {
            var qh = new QueryHelper<Notification>(connection_string, (r) => new Notification()
            {
                Id = r.GetInt32(0),
                BadgeId =  r.IsDBNull(1) ?  null : r.GetInt32(2),
                OwnerId = r.GetInt32(2),
                Title = r.GetString(3),
                Body = r.GetString(4),
                Created = r.GetDateTime(5),
                Updated = r.GetDateTime(6),
            });
            qh.AddParameter(":notification_id", notificationId);

            return qh.ExecuteQueryToSingleObject(
                "select id, badge_id , owner_id, title, body, created , updated from  notifications where id = :notification_id");
        }

        public IEnumerable<Notification> GetUserNotificationsFeed(NotificationFilter filter)
        {
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
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

            return qh.ExecuteQueryToObjectList(sql);
        }

        private void CreateTable()
        {
            var rh = new RevisionHelper(connection_string, "notifications");
            var qh = new QueryHelper<Notification>(connection_string, FromNotificationReader);
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

            

        }

        private Notification FromNotificationReader(NpgsqlDataReader reader)
        {
            var obj = new Notification();

            obj.Id = reader.GetInt32(0);
            if (!reader.IsDBNull(1))
            {
                obj.BadgeId =  reader.GetInt32(1);
            }
            obj.OwnerId = reader.GetInt32(2);
            obj.Title = reader.GetString(3);
            obj.Body = reader.GetString(4);
            obj.Created = reader.GetDateTime(5);
            obj.Updated = reader.GetDateTime(6);

            var owner = new User();
            owner.Firstname = reader.GetString(7);
            owner.Lastname = reader.GetString(8);
            owner.Nickname = reader.GetString(9);
            obj.Owner = owner;

            obj.Liked = !reader.IsDBNull(10) && reader.GetBoolean(10);
            obj.Shared = !reader.IsDBNull(11) && reader.GetBoolean(11);
            obj.Seen = !reader.IsDBNull(12) && reader.GetBoolean(12);

            return obj;
        }
    }
}