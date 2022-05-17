using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class UserStatistics
    {
        public int UserId { get; set; }
        public int TotalNetworkChallengesCount { get; set; }
        public int TotalOrganizingChallengesCount { get; set; }
        public int TotalLearningChallengesCount { get; set; }

        public int TotalAcceptedChallenges
        {
            get { return TotalNetworkChallengesCount +
                    TotalOrganizingChallengesCount +
                    TotalLearningChallengesCount; }
        }

        public int TotalNetworkChallengesPoints { get; set; }
        public int TotalOrganizingChallengesPoints { get; set; }
        public int TotalLearningChallengesPoints { get; set; }

        public int TotalPoints
        {
            get { return TotalLearningChallengesPoints +
                    TotalOrganizingChallengesPoints +
                    TotalLearningChallengesPoints; }
        }
        public IEnumerable<RankEntry> FriendsRank { get; set; }

        public int LastWeekChallengeCount { get; set; }
        public int ThisWeekChallengeCount { get; set; }
        public int LastMonthChallengeCount { get; set; }
        public int ThisMonthChallengeCount { get; set; }
        /*
        public StatisticsTrend WeeklyStatisticsTrend { get; set; }
        public StatisticsTrend MonthlyStatisticsTrend { get; set; }

        public void AddStatisticTrends()
        {
            try
            {
                WeeklyStatisticsTrend = new StatisticsTrend(LastWeekChallengeCount, ThisWeekChallengeCount);
                MonthlyStatisticsTrend = new StatisticsTrend(LastMonthChallengeCount, ThisMonthChallengeCount);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"StatisticTrend could not be calculated. Maybe missing Statistics? {e}");
            }
        }
        */
    }
}