using System;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model.Enum;

namespace StudyBuddy.App.Api
{
    public delegate void LoginStateChangedHandler(object sender, LoginStateChangedArgs args);

    public class LoginStateChangedArgs : EventArgs
    {
        public LoginStateChangedArgs(bool is_logged_in, UserViewModel user, string token)
        {
            IsLoggedIn = is_logged_in;
            IsLoggedOut = !is_logged_in;
            User = user;
            Token = token;
        }

        public bool IsLoggedIn { get; }
        public bool IsLoggedOut { get; }
        public UserViewModel User { get; }
        public string Token { get; set; }
    }

    public interface IAuthenticationService
    {
        string Token { get; }
        UserViewModel CurrentUser { get; }
        bool IsLoggedIn { get; }
        Task<LoginStatus> Login(UserCredentials credentials);
        Task<bool> LoginFromJson(string content);
        void Logout();
        event LoginStateChangedHandler LoginStateChanged;
        Task<bool> IsTokenValid(string token);
        Task<bool> SendPasswortResetMail(string email);
    }
}