using StudyBuddy.Model;
using System.Collections.Generic;

namespace StudyBuddy.BusinessLogic.Interfaces
{
    public interface IStatisticsService
    {
        UserStatistics GetUserStatistics(int user_id);
        IEnumerable<User> GetUsersWithDateCreated(bool orderAscending);
        IEnumerable<Challenge> GetChallengeStatistic(bool orderAscending);
        IEnumerable<GameBadge> GetBadgeStatistics(bool orderAscending);
        IEnumerable<GameObjectStatistics> GetChallengeCompletionStatistics(bool orderAscending);
        IEnumerable<GameObjectStatistics> GetBadgeCompletionStatistics(bool orderAscending);
    }
}
