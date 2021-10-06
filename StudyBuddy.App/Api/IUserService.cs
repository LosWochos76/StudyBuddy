using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetFriends(bool reload = false);
    }
}