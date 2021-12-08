using System.Collections.Generic;

namespace StudyBuddy.Model.Model
{
    public class UserStatistics
    {
        public int UserId { get; set; }
        public int TotalNetworkChallengesCount { get; set; }
        public int TotalOrganizingChallengesCount { get; set; }
        public int TotalLearningChallengesCount { get; set; }
        public int TotalAcceptedChallenges
        {
            get { return TotalNetworkChallengesCount + TotalOrganizingChallengesCount + TotalLearningChallengesCount; }
        }


        public int TotalNetworkChallengesPoints { get; set; }
        public int TotalOrganizingChallengesPoints { get; set; }
        public int TotalLearningChallengesPoints { get; set; }
        public int TotalPoints
        {
            get { return TotalLearningChallengesPoints + TotalOrganizingChallengesPoints + TotalLearningChallengesPoints; }
        }

        public IEnumerable<RankEntry> FriendsRank{ get; set; }
    }
}
