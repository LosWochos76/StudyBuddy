using StudyBuddy.App.ViewModels;
using System.Threading.Tasks;

namespace StudyBuddy.App.Api
{
    public interface IBadgeService
    {
        Task<GameBadgeViewModel> GetById(int badge_id);
        Task<GameBadgeListViewModel> Accepted(string search_string = "", int skip = 0);
        Task<GameBadgeListViewModel> BadgeReceived(string search_string = "", int skip = 0);
    }
}