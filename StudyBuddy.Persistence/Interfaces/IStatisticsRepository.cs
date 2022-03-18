using StudyBuddy.Model;
using System.Collections.Generic;

namespace StudyBuddy.Persistence
{
    public interface IStatisticsRepository
    {
        UserStatistics GetUserStatistics(int user_id);
        IEnumerable<RankEntry> GetRankingWithFriends(int user_id);
        IEnumerable<Result> GetResult(int user_id);
    }
}