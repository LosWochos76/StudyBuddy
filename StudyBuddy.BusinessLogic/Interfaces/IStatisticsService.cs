using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic.Interfaces
{
    public interface IStatisticsService
    {
        UserStatistics GetUserStatistics(int user_id);
    }
}
