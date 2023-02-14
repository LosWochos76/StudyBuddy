using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class FcmTokenConverter : BaseConverter<FcmToken>
    {
        public override FcmToken Convert(DataSet set, int row)
        {
            var obj = new FcmToken();
            obj.ID = set.GetInt(row, "id");
            obj.Created = set.GetDateTime(row, "created");
            obj.Token = set.GetString(row, "token");
            obj.UserID = set.GetInt(row, "user_id");
            obj.Created = set.GetDateTime(row, "created");
            obj.LastSeen = set.GetDateTime(row, "last_seen");
            return obj;
        }
    }
}

