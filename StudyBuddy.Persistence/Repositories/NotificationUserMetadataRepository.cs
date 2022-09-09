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

        public void Upsert(NotificationUserMetadata obj)
        {
            var metadata = FindNotificationUserMetadata(obj);
            if (metadata is null)
                Insert(obj);
            else
                Update(obj);
        }

        public void Insert(NotificationUserMetadata insert)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":notificationId", insert.NotificationId);
            qh.AddParameter(":ownerId", insert.OwnerId);
            qh.AddParameter(":liked", insert.Liked);
            qh.AddParameter(":seen", insert.Seen);
            qh.AddParameter(":shared", insert.Shared);
            qh.ExecuteNonQuery("insert into notification_user_metadata (notification_id, owner_id, " +
                "liked, seen, shared) values (:notificationId, :ownerId, :liked, :seen, :shared) returning id");
        }

        public void Update(NotificationUserMetadata update)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":liked", update.Liked);
            qh.AddParameter(":seen", update.Seen);
            qh.AddParameter(":shared", update.Shared);
            qh.AddParameter(":id", update.Id);
            var sql = "update notification_user_metadata set " +
                "liked=:liked, seen=:seen, shared=:shared where id=:id";
            qh.ExecuteNonQuery(sql);
        }

        public NotificationUserMetadata FindNotificationUserMetadata(NotificationUserMetadata obj)
        {
            return FindNotificationUserMetadata(obj.NotificationId, obj.OwnerId);
        }

        public NotificationUserMetadata FindNotificationUserMetadata(int notification_id, int owner_id)
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

        public IEnumerable<NotificationUserMetadata> GetAllUnseen()
        {
            var qh = new QueryHelper(connection_string);
            var sql = "select id, notification_id, owner_id, liked, seen, shared, created, updated " +
                "from notification_user_metadata where seen=false";
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