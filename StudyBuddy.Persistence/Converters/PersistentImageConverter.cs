using System;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class PersistentImageConverter : BaseConverter<PersistentImage>
    {
        public override PersistentImage Convert(DataSet set, int row)
        {
            var obj = new PersistentImage();
            obj.ID = set.GetInt(row, "id");
            obj.UserID = set.GetInt(row, "user_id");
            obj.Name = set.GetString(row, "name");
            obj.Length = set.GetLong(row, "length");
            obj.Content = set.GetByteArray(row, "content");
            return obj;
        }
    }
}