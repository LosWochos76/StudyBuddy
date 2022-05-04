using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
namespace StudyBuddy.App.Api
{
    public interface IBadgeService
    {
        Task<GameBadgeViewModel> GetById(int challenge_id);
        Task<GameBadgeListViewModel> Accepted(string search_string = "", int skip = 0);
    }
}