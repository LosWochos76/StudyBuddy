using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class BadgeSuccessRateConverter : BaseConverter<BadgeSuccessRate>
    {
        public override BadgeSuccessRate Convert(DataSet set, int row)
        {
            var obj = new BadgeSuccessRate();
            obj.BadgeId = set.GetInt(row, "badge_id");
            obj.UserId = set.GetInt(row, "user_id");
            obj.OverallChallengeCount = set.GetInt(row, "overall_challenge_count");
            obj.AcceptedChallengeCount = set.GetInt(row, "accepted_challenge_count");
            return obj;
        }
    }
}