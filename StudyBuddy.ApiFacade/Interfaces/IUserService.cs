using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.ApiFacade
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetFriends();
    }
}
