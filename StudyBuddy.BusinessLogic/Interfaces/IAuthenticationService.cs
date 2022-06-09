namespace StudyBuddy.BusinessLogic
{
    public interface IAuthenticationService
    {
        object Login(UserCredentials uc);
        void SendMail(string email, bool forgotpassword);
        bool CheckToken(string token);
        bool CheckPasswordResetToken(string token, string email);
    }
}