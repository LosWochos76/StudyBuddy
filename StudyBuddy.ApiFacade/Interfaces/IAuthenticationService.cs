using System;
using System.Threading.Tasks;
using StudyBuddy.ApiFacade.Restful;
using StudyBuddy.Model;

namespace StudyBuddy.ApiFacade
{
    public delegate void LoginStateChangedHandler(object sender, LoginStateChangedArgs args);

    public class LoginStateChangedArgs : EventArgs
    {
        public bool IsLoggedIn { get; private set; }
        public bool IsLoggedOut { get; private set; }
        public User User { get; private set; }
        public string Token { get; set; }

        public LoginStateChangedArgs(bool is_logged_in, User user, string token)
        {
            this.IsLoggedIn = is_logged_in;
            this.IsLoggedOut = !is_logged_in;
            this.User = user;
            this.Token = token;
        }
    }

    public interface IAuthenticationService
    {
        string Token { get; }
        User CurrentUser { get; }
        bool IsLoggedIn { get; }
        Task<bool> Login(UserCredentials credentials);
        Task<bool> LoginFromJson(string content);
        void Logout();
        event LoginStateChangedHandler LoginStateChanged;
    }
}