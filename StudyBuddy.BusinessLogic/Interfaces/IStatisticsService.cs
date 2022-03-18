using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic.Interfaces
{
    public interface IStatisticsService
    {
        UserStatistics GetUserStatistics(int user_id);
        IEnumerable<RankEntry> GetFriendsRanks(int user_id);
        Score GetScore(int user_id);
    }
}
