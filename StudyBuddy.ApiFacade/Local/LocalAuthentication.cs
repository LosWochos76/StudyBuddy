using System;
using System.Threading.Tasks;
using StudyBuddy.Model;
using StudyBuddy.ApiFacade;
using StudyBuddy.ApiFacade.Restful;

[assembly: Xamarin.Forms.Dependency(typeof(LocalAuthentication))]
namespace StudyBuddy.ApiFacade
{
    public class LocalAuthentication : IAuthentication
    {
        public string Token { get; private set; }
        public User CurrentUser { get; private set; }

        public bool IsLoggedIn { get; private set; }

        public async Task<bool> Login(UserCredentials credentials)
        {
            throw new NotImplementedException();
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
