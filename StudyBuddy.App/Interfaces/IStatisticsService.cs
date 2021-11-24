using System.Threading.Tasks;
using StudyBuddy.Model.Model;

namespace StudyBuddy.App.Interfaces
{
    public interface IStatisticsService
    {
        Task<UserStatistics> GetUserStatistics(int user_id);
    }
}
