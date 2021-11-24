using StudyBuddy.Model.Model;

namespace StudyBuddy.BusinessLogic.Interfaces
{
    public interface IStatisticsService
    {
        public UserStatistics GetUserStatistics(int user_id);
    }
}
