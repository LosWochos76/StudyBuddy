using System;
using System.Threading.Tasks;
using StudyBuddy.Model;
using StudyBuddy.ServiceFacade;

[assembly: Xamarin.Forms.Dependency(typeof(LocalAuthentication))]
namespace StudyBuddy.ServiceFacade
{
    public class LocalAuthentication : IAuthentication
    {
        public string Token { get; private set; }
        public User CurrentUser { get; private set; }

        public bool IsLoggedIn { get; private set; }

        public async Task<bool> Login(UserCredentials credentials)
        {
            IsLoggedIn = true;
            return true;
        }

        public async Task<bool> LoginFromJson(string content)
        {
            IsLoggedIn = true;
            return true;
        }

        public void Logout()
        {
            IsLoggedIn = false;
        }
    }
}
