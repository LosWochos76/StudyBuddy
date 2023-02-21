using StudyBuddy.Model;
using System.Collections.Generic;

namespace StudyBuddy.Persistence
{
    public interface IStatisticsRepository
    {
        UserStatistics GetUserStatistics(int user_id);
        IEnumerable<User> GetUsersWithDateCreated(bool orderAscending);
        IEnumerable<Challenge> GetChallengeStatistic(bool orderAscending);
        IEnumerable<GameBadge> GetBadgeStatistic(bool orderAscending);
        IEnumerable<GameObjectStatistics> GetChallengeCompletionStatistics(bool orderAscending);
        IEnumerable<GameObjectStatistics> GetBadgeCompletionStatistics(bool orderAscending);
    }
}