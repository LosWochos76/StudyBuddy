using System;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class GameBadgeConverter : BaseConverter<GameBadge>
    {
        public override GameBadge Convert(DataSet set, int row)
        {
            var obj = new GameBadge();
            obj.ID = set.GetInt(row, "id");
            obj.Created = set.GetDateTime(row, "created");
            obj.Name = set.GetString(row, "name");
            obj.OwnerID = set.GetInt(row, "owner_id");
            obj.RequiredCoverage = set.GetDouble(row, "required_coverage");
            obj.Description = set.GetString(row, "description");
            obj.IconKey = set.GetString(row, "iconkey");
            obj.Tags = set.GetString(row, "tags_of_badge");
            obj.Received = set.GetDateTime(row, "received");
            return obj;
        }
    }
}

