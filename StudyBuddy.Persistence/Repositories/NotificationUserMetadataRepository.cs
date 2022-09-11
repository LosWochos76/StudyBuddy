using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    class NotificationUserMetadataRepository : INotificationUserMetadataRepository
    {
        private readonly string connection_string;
        private NotificationUserMetaDataConverter converter = new NotificationUserMetaDataConverter();

        public NotificationUserMetadataRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {
            var qh = new QueryHelper(connection_string);
            if (!qh.TableExists("notification_user_metadata"))
                qh.ExecuteNonQuery(
                    "create table notification_user_metadata (" +
                    "id serial primary key, " +
                    "notification_id int not null references notifications(id), " +
                    "owner_id int not null references users(id), " +
                    "liked boolean default false , " +
                    "seen boolean default false , " +
                    "shared boolean default false , " +
                    "created timestamp default current_timestamp, " +
                    "updated timestamp default current_timestamp " +
                    ")");
        }

        public void Insert(NotificationUserMetadata insert)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":notificationId", insert.NotificationId);
            qh.AddParameter(":ownerId", insert.OwnerId);
            qh.AddParameter(":created", insert.Created);
            qh.AddParameter(":updated", insert.Updated);

            qh.AddParameter(":liked", insert.Liked ?? false);
            qh.AddParameter(":seen", insert.Seen ?? false);
            qh.AddParameter(":shared", insert.Shared ?? false);

            qh.ExecuteNonQuery("insert into notification_user_metadata (notification_id, owner_id, " +
                "liked, seen, shared, created, updated) values (:notificationId, :ownerId, :liked, :seen, :shared, :created, :updated) returning id");
        }

        public void Update(NotificationUserMetadata obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":id", obj.Id);
            qh.AddParameter(":created", obj.Created);
            qh.AddParameter(":updated", obj.Updated);
            var sql = "update notification_user_metadata set created=:created, updated=:updated ";

            if (obj.Liked.HasValue)
            {
                qh.AddParameter(":liked", obj.Liked.Value);
                sql += ", liked=:liked";
            }

            if (obj.Shared.HasValue)
            {
                qh.AddParameter(":shared", obj.Shared);
                sql += ", shared=:shared";
            }

            if (obj.Seen.HasValue)
            {
                qh.AddParameter(":seen", obj.Seen);
                sql += ", seen=:seen";
            }

            sql += " where id=:id";
            qh.ExecuteNonQuery(sql);
        }

        public NotificationUserMetadata FindByNotificationAndOwner(int notification_id, int owner_id)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":notification_id", notification_id);
            qh.AddParameter(":owner_id", owner_id);
            var sql = "select id, notification_id, owner_id, liked, seen, shared, created, " +
                "updated from notification_user_metadata where notification_id=:notification_id and owner_id=:owner_id";
            var set = qh.ExecuteQuery(sql);
            return converter.Single(set);
        }

        public IEnumerable<NotificationUserMetadata> GetAll()
        {
            var qh = new QueryHelper(connection_string);
            var sql = "select id, notification_id, owner_id, liked, seen, shared, created, updated from notification_user_metadata";
            var set = qh.ExecuteQuery(sql);
            return converter.Multiple(set);
        }

        public void DeleteAllForNotification(int notification_id)
        {
            var qh = new QueryHelper(connection_string);
            var sql = "delete from notification_user_metadata where notification_id=:notification_id";
            qh.AddParameter(":notification_id", notification_id);
            qh.ExecuteNonQuery(sql);
        }
    }
}