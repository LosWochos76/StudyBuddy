using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IRequestService
    {
        Task<bool> AskForFriendship(UserViewModel obj);
        Task<bool> AskForChallengeAcceptance(int other_user_id, int challenge_id);
        Task<RequestListViewModel> ForMe();
        Task<RequestListViewModel> FromMe();
        Task<bool> Accept(RequestViewModel request);
        Task<bool> Deny(RequestViewModel request);
        Task<bool> DeleteFriendshipRequest(UserViewModel obj);
    }
}