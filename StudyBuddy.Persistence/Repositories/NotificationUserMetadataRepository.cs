using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class NotificationUserMetadataRepository
    {
        private readonly string connection_string;

        public NotificationUserMetadataRepository(string connection_string)
        {
            this.connection_string = connection_string;

            CreateTable();
        }

        private void CreateTable()
        {
            var qh = new QueryHelper<NotificationUserMetadata>(connection_string, FromNotificationUserMetadataReader);
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

        private NotificationUserMetadata FromNotificationUserMetadataReader(NpgsqlDataReader reader)
        {
            var obj = new NotificationUserMetadata();

            obj.Id = reader.GetInt32(0);
            obj.NotificationId = reader.GetInt32(1);
            obj.OwnerId = reader.GetInt32(2);
            obj.Liked = reader.GetBoolean(3);
            obj.Seen = reader.GetBoolean(4);
            obj.Shared = reader.GetBoolean(5);
            obj.Created = reader.GetDateTime(6);
            obj.Updated = reader.GetDateTime(7);

            return obj;
        }

        public void Upsert(NotificationUserMetadataUpsert upsert)
        {
            var metadata = FindNotificationUserMetadata(upsert);

            if (metadata is null)
                Insert(new NotificationUserMetadataInsert
                {
                    NotificationId = upsert.NotificationId,
                    OwnerId = upsert.OwnerId,
                    Liked = upsert.Liked,
                    Seen = upsert.Seen,
                    Shared = upsert.Shared
                });
            else
                Update(new NotificationUserMetadataUpdate
                {
                    Id = metadata.Id,
                    Liked = upsert.Liked,
                    Seen = upsert.Seen,
                    Shared = upsert.Shared
                });
        }

        public void Insert(NotificationUserMetadataInsert insert)
        {
            var qh = new QueryHelper<NotificationUserMetadata>(connection_string, FromNotificationUserMetadataReader);
            qh.AddParameter(":notificationId", insert.NotificationId);
            qh.AddParameter(":ownerId", insert.OwnerId);
            qh.AddParameter(":liked", insert.Liked ?? false);
            qh.AddParameter(":seen", insert.Seen ?? false);
            qh.AddParameter(":shared", insert.Shared ?? false);


            qh.ExecuteNonQuery(
                "insert into notification_user_metadata (notification_id, owner_id, liked, seen, shared) values (:notificationId, :ownerId, :liked, :seen, :shared) returning id");
        }

        public void Update(NotificationUserMetadataUpdate update)
        {
            var qh = new QueryHelper<NotificationUserMetadata>(connection_string, FromNotificationUserMetadataReader);

            var sql = "update notification_user_metadata set ";

            if (update.Liked is not null)
            {
                qh.AddParameter(":liked", update.Liked);
                sql += "liked=:liked, ";
            }

            if (update.Seen is not null)
            {
                qh.AddParameter(":seen", update.Seen);
                sql += "seen=:seen, ";
            }

            if (update.Shared is not null)
            {
                qh.AddParameter(":shared", update.Shared);
                sql += "shared=:shared, ";
            }


            qh.AddParameter(":id", update.Id);
            sql += "id=:id where id=:id";


            qh.ExecuteNonQuery(sql);
        }


        public NotificationUserMetadata FindNotificationUserMetadata(NotificationUserMetadataUpsert upsert)
        {
            return FindNotificationUserMetadata(upsert.NotificationId, upsert.OwnerId);
        }

        public NotificationUserMetadata FindNotificationUserMetadata(int notificationId, int ownerId)
        {
            var qh = new QueryHelper<NotificationUserMetadata>(connection_string, FromNotificationUserMetadataReader);
            qh.AddParameter(":notification_id", notificationId);
            qh.AddParameter(":owner_id", ownerId);

            var sql =
                "select id, notification_id, owner_id, liked, seen, shared, created, updated from notification_user_metadata where notification_id=:notification_id and owner_id=:owner_id";

            var metadata = qh.ExecuteQueryToSingleObject(sql);
            return metadata;
        }

        public IEnumerable<NotificationUserMetadata> All()
        {
            var qh = new QueryHelper<NotificationUserMetadata>(connection_string, FromNotificationUserMetadataReader);
            var sql =
                "select id, notification_id, owner_id, liked, seen, shared, created, updated from notification_user_metadata";
            var metadatas = qh.ExecuteQueryToObjectList(sql);
            return metadatas;
        }
    }
}