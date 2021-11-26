using StudyBuddy.Model.Model;

namespace StudyBuddy.Persistence
{
    public interface IStatisticsRepository
    {
        UserStatistics GetUserStatistics(int user_id);

    }
}
