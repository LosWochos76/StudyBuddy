using System;
using System.Threading.Tasks;
using StudyBuddy.Model;

namespace StudyBuddy.App.Api
{
    public delegate void LoginStateChangedHandler(object sender, LoginStateChangedArgs args);

    public class LoginStateChangedArgs : EventArgs
    {
        public LoginStateChangedArgs(bool is_logged_in, User user, string token)
        {
            IsLoggedIn = is_logged_in;
            IsLoggedOut = !is_logged_in;
            User = user;
            Token = token;
        }

        public bool IsLoggedIn { get; }
        public bool IsLoggedOut { get; }
        public User User { get; }
        public string Token { get; set; }
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