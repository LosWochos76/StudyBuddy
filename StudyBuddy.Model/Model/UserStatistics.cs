using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class UserStatistics
    {
        public int UserId { get; set; }
        public int TotalNetworkChallengesCount { get; set; }
        public int TotalOrganizingChallengesCount { get; set; }
        public int TotalLearningChallengesCount { get; set; }

        public int TotalNetworkChallengesPoints { get; set; }
        public int TotalOrganizingChallengesPoints { get; set; }
        public int TotalLearningChallengesPoints { get; set; }

        public int TotalPoints =>
            TotalLearningChallengesPoints +
            TotalOrganizingChallengesPoints +
            TotalLearningChallengesPoints;

        public IEnumerable<RankEntry> FriendsRank { get; set; }

        public int LastWeekChallengeCount { get; set; }
        public int ThisWeekChallengeCount { get; set; }
        public int LastMonthChallengeCount { get; set; }
        public int ThisMonthChallengeCount { get; set; }
    }
}