using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class NotificationConverter : BaseConverter<Notification>
    {
        public override Notification Convert(DataSet set, int row)
        {
            var obj = new Notification();

            obj.Id = set.GetInt(row, "id");
            obj.BadgeId = set.GetInt(row, "badge_id");
            obj.OwnerId = set.GetInt(row, "owner_id");
            obj.Title = set.GetString(row, "title");
            obj.Body = set.GetString(row, "body");
            obj.Created = set.GetDateTime(row, "created");
            obj.Updated = set.GetDateTime(row, "updated");

            var owner = new User();
            owner.Firstname = set.GetString(row, "firstname");
            owner.Lastname = set.GetString(row, "lastname");
            owner.Nickname = set.GetString(row, "nickname");
            obj.Owner = owner;

            obj.Liked = set.GetBool(row, "liked");
            obj.Shared = set.GetBool(row, "shared");
            obj.Seen = set.GetBool(row, "seen");
            return obj;
        }
    }
}

