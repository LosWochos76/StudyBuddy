namespace StudyBuddy.BusinessLogic
{
    public interface IAuthenticationService
    {
        object Login(UserCredentials uc);
        void SendPasswortResetMail(string email);
        bool CheckToken(string token);
        bool CheckPasswordResetToken(string token, string email);
    }
}