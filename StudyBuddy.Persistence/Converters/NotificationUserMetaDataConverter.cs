using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class NotificationUserMetaDataConverter : BaseConverter<NotificationUserMetadata>
    {
        public override NotificationUserMetadata Convert(DataSet set, int row)
        {
            var obj = new NotificationUserMetadata();
            obj.Id = set.GetInt(row, "id");
            obj.NotificationId = set.GetInt(row, "notification_id");
            obj.OwnerId = set.GetInt(row, "owner_id");
            obj.Liked = set.GetBool(row, "liked");
            obj.Seen = set.GetBool(row, "seen");
            obj.Shared = set.GetBool(row, "shared");
            obj.Created = set.GetDateTime(row, "created");
            obj.Updated = set.GetDateTime(row, "updated");
            return obj;
        }
    }
}

