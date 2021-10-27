using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface ITagService
    {
        Task<TagListViewModel> OfChallenge(int challenge_id);
        Task<TagListViewModel> OfBadge(int badge_id);
    }
}
