using System.Text.Json.Serialization;

namespace StudyBuddy.Model
{
    public class BadgeSuccessRate
    {
        public int BadgeId { get; set; }
        public int UserId { get; set; }
        public int OverallChallengeCount { get; set; }
        public int AcceptedChallengeCount { get; set; }

        [JsonIgnore]
        public double Success
        {
            get
            {
                return OverallChallengeCount > 0 ? (double)AcceptedChallengeCount / OverallChallengeCount : 0;
            }
        }
    }
}