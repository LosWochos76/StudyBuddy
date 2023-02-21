using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class GameObjectStatisticsConverter : BaseConverter<GameObjectStatistics>
    {
        public override GameObjectStatistics Convert(DataSet set, int row)
        {
            GameObjectStatistics obj = new();
            obj.UserID = set.GetInt(row, "user_id");
            if(set.HasColumn("badge_id"))
            {
                obj.ItemID = set.GetInt(row, "badge_id");
            }
            if(set.HasColumn("challenge_id"))
            {
                obj.ItemID = set.GetInt(row, "challenge_id");
            }
            obj.DateCompleted = set.GetDateTime(row, "created");
            return obj;
        }
    }
}
