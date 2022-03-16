using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.App.Interfaces
{
    public interface IStatisticsService
    {
        Task<UserStatistics> GetUserStatistics();
        Task<UserStatistics> GetUserStatisticsForUser(int userId);
    }
}
