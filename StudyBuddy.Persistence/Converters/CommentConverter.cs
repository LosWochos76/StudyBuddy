using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class CommentConverter : BaseConverter<Comment>
    {
        public override Comment Convert(DataSet set, int row)
        {
            var obj = new Comment();
            obj.Id = set.GetInt(row, "id");
            obj.NotificationId = set.GetInt(row, "notification_id");
            obj.OwnerId = set.GetInt(row, "owner_id");
            obj.Text = set.GetString(row, "text");
            obj.Created = set.GetDateTime(row, "created");
            obj.Updated = set.GetDateTime(row, "updated");
            obj.Owner = new User();
            obj.Owner.ID = set.GetInt(row, "user_id");
            obj.Owner.Firstname = set.GetString(row, "firstname");
            obj.Owner.Lastname = set.GetString(row, "lastname");
            obj.Owner.Nickname = set.GetString(row, "nickname");
            obj.Owner.Email = set.GetString(row, "email");
            obj.Owner.Role = (Role)set.GetInt(row, "role");
            return obj;
        }
    }
}

