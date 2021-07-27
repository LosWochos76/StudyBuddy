using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.ServiceFacade
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