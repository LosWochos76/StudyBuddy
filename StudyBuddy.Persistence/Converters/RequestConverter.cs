using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class RequestConverter : BaseConverter<Request>
    {
        public override Request Convert(DataSet set, int row)
        {
            var obj = new Request();
            obj.ID = set.GetInt(row, "id");
            obj.Created = set.GetDateTime(row, "created");
            obj.SenderID = set.GetInt(row, "sender_id");
            obj.RecipientID = set.GetInt(row, "recipient_id");
            obj.Type = (RequestType)set.GetInt(row, "type");
            obj.ChallengeID = set.GetInt(row, "challenge_id");
            return obj;
        }
    }
}

