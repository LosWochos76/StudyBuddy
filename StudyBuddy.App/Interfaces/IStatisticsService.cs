using StudyBuddy.App.Models;
using System.Threading.Tasks;

namespace StudyBuddy.App.Interfaces
{
    public interface IStatisticsService
    {
        Task<UserStatistics> GetUserStatistics();
    }
}
