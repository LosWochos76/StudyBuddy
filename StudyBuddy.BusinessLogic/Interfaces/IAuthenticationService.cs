using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IAuthenticationService
    {
        LoginResult Login(UserCredentials uc);
        void SendMail(string email, bool forgotpassword);
        bool CheckToken(string token);
        bool CheckPasswordResetToken(string token, string email);
        string GenerateUserToken(string email);
        bool CheckForValidMail(string email);
        void SendPasswordResetMail(string email);
    }
}