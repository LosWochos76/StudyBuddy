using StudyBuddy.App.ViewModels;
using StudyBuddy.Model.Model;
using System.Threading.Tasks;

namespace StudyBuddy.App.Interfaces
{
    public interface IStatisticsService
    {
        Task<UserStatistics> GetUserStatistics(int userId);
        Task<UserStatistics> GetUserStatisticsForUser(int userId);
    }
}
