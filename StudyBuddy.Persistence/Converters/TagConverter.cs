using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class TagConverter : BaseConverter<Tag>
    {
        public override Tag Convert(DataSet set, int row)
        {
            var obj = new Tag();
            obj.ID = set.GetInt(row, "id");
            obj.Created = set.GetDateTime(row, "created");
            obj.Name = set.GetString(row, "name");
            return obj;
        }
    }
}

