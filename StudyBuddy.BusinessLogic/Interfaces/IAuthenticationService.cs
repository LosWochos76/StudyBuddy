﻿namespace StudyBuddy.BusinessLogic
{
    public interface IAuthenticationService
    {
        object Login(UserCredentials uc);
        void SendPasswortResetMail(string email);
    }
}