using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class NotificationConverter : BaseConverter<Notification>
    {
        public override Notification Convert(DataSet set, int row)
        {
            var obj = new Notification();
            obj.ID = set.GetInt(row, "id");
            obj.Created = set.GetDateTime(row, "created");
            obj.BadgeId = set.GetInt(row, "badge_id");
            obj.OwnerId = set.GetInt(row, "owner_id");
            obj.Title = set.GetString(row, "title");
            obj.Body = set.GetString(row, "body");
            obj.Created = set.GetDateTime(row, "created");
            obj.Updated = set.GetDateTime(row, "updated");
            return obj;
        }
    }
}