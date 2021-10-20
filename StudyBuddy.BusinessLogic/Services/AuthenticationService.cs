using System;
using NETCore.MailKit;
using NETCore.MailKit.Core;

namespace StudyBuddy.BusinessLogic
{
    class AuthenticationService : IAuthenticationService
    {
        private readonly IBackend backend;

        public AuthenticationService(IBackend backend)
        {
            this.backend = backend;
        }

        public object Login(UserCredentials uc)
        {
            if (uc == null || string.IsNullOrEmpty(uc.EMail) || string.IsNullOrEmpty(uc.Password))
                throw new Exception("Provide email and password!");

            var user = backend.Repository.Users.ByEmailAndPassword(uc.EMail, uc.Password);
            if (user == null)
                return null;

            user.PasswordHash = null;
            var jwt = new JwtToken(backend.Repository);
            return new
            {
                Token = jwt.FromUser(user),
                User = user
            };
        }

        public void SendPasswortResetMail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("No email-adress given!");

            var obj = backend.Repository.Users.ByEmail(email);
            if (obj == null)
                throw new Exception("User not found!");

            // ToDo: Hier muss noch mehr gemacht werden!

            var options = MailKitHelper.GetMailKitOptions();
            var provider = new MailKitProvider(options);
            var mail = new EmailService(provider);
            mail.Send(email, "Passwort zurücksetzen", "Bla", true);
        }
    }
}