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
        Task<IEnumerable<RequestViewModel>> ForMe();
        Task<bool> Accept(int request_id);
        Task<bool> Deny(int request_id);
    }
}