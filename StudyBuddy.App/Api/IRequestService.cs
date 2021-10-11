using System;
using System.Threading.Tasks;

namespace StudyBuddy.App.Api
{
    public interface IRequestService
    {
        Task<bool> AskForFriendship(int other_user_id);
    }
}