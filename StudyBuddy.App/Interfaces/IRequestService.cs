using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IRequestService
    {
        Task<bool> AskForFriendship(int other_user_id);
        Task<bool> AskForChallengeAcceptance(int other_user_id, int challenge_id);
        Task ForMe(ObservableCollection<RequestViewModel> list, bool reload = false);
        Task FromMe(ObservableCollection<RequestViewModel> list, bool reload = false);
        Task<bool> Accept(RequestViewModel request);
        Task<bool> Deny(RequestViewModel request);
        Task<RequestViewModel> GetFriendshipRequest(int other_user_id);
        Task<bool> Delete(RequestViewModel request);
    }
}