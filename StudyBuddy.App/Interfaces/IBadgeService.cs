using StudyBuddy.App.ViewModels;
using System.Threading.Tasks;

namespace StudyBuddy.App.Api
{
    public interface IBadgeService
    {
        Task<GameBadgeListViewModel> ForToday(string search_string = "", int skip = 0);
    }
}