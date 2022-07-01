using StudyBuddy.Model;
using System.Collections.Generic;

namespace StudyBuddy.Persistence
{
    public interface IStatisticsRepository
    {
        UserStatistics GetUserStatistics(int user_id);
    }
}