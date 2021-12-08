using System.Collections.Generic;
using StudyBuddy.Model.Model;

namespace StudyBuddy.BusinessLogic.Interfaces
{
    public interface IStatisticsService
    {
        UserStatistics GetUserStatistics(int user_id);
        IEnumerable<RankEntry> GetFriendsRanks(int user_id);
    }
}
