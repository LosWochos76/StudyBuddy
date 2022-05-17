using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Interfaces
{
    public interface IBadgeService
    {
        Task<GameBadgeViewModel> GetById(int badge_id);
        Task<GameBadgeListViewModel> Accepted(string search_string = "", int skip = 0);
    }
}