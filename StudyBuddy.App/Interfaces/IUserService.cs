using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public interface IUserService
    {
        Task<UserListViewModel> GetFriends(string search_string = "", int skip = 0);
        Task<UserListViewModel> GetNotFriends(string search_string = "", int skip = 0);
        Task<bool> RemoveFriend(UserViewModel uvm);
        Task<UserViewModel> GetById(int user_id);
        Task<int> GetFriendsCount();
        Task<bool> Update(User uvm);
        Task<UserId> IdByEmail(string email);
        Task<UserId> IdByNickname(string nickname);
        Task<UserViewModel> Register(User new_user);
    }
}