using System.Threading.Tasks;
using StudyBuddy.ApiFacade.Restful;
using StudyBuddy.Model;

namespace StudyBuddy.ApiFacade
{
    public interface IAuthentication
    {
        string Token { get; }
        User CurrentUser { get; }
        bool IsLoggedIn { get; }

        Task<bool> Login(UserCredentials credentials);
        Task<bool> LoginFromJson(string content);
        void Logout();
    }
}