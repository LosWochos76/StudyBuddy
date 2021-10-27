using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IUserService
    {
        Task GetFriends(ObservableCollection<UserViewModel> list, string search_text, bool reload = false);
        Task GetNotFriends(ObservableCollection<UserViewModel> list, string search_text, bool reload = false);
        Task<bool> RemoveFriend(int friend_id);
        Task<UserViewModel> GetById(int user_id);
    }
}