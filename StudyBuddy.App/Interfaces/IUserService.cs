using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetFriends(string search_string = "", int skip = 0);
        Task<IEnumerable<UserViewModel>> GetNotFriends(string search_string = "", int skip = 0);
        Task<bool> RemoveFriend(UserViewModel uvm);
        Task<UserViewModel> GetById(int user_id);
        Task<int> GetFriendsCount();
        Task<bool> Update(User uvm);
    }
}