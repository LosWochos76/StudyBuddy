using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IRequestService
    {
        Task<bool> AskForFriendship(int other_user_id);
        Task<bool> AskForChallengeAcceptance(int other_user_id, int challenge_id);
        Task<IEnumerable<RequestViewModel>> ForMe(bool reload = false);
        Task<IEnumerable<RequestViewModel>> FromMe(bool reload = false);
        Task<bool> Accept(int request_id);
        Task<bool> Deny(int request_id);
        Task<RequestViewModel> GetFriendshipRequest(int other_user_id);
        Task<bool> Delete(int request_id);
    }
}