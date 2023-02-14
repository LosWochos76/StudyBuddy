using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class BusinessEventConverter : BaseConverter<BusinessEvent>
    {
        public override BusinessEvent Convert(DataSet set, int row)
        {
            var obj = new BusinessEvent();
            obj.ID = set.GetInt(row, "id");
            obj.Created = set.GetDateTime(row, "created");
            obj.Name = set.GetString(row, "name");
            obj.OwnerID = set.GetInt(row, "owner_id");
            obj.Type = (BusinessEventType)set.GetInt(row, "type");
            obj.Code = set.GetString(row, "code");
            return obj;
        }
    }
}

