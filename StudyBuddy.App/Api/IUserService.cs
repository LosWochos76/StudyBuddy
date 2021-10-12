using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetFriends(bool reload = false);
        Task<bool> RemoveFriend(int friend_id);
        Task<IEnumerable<UserViewModel>> GetNotFriends(string search_text, bool reload = false);
        Task<UserViewModel> GetById(int user_id);
    }
}