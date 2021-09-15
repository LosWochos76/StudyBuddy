using System;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class AuthenticationService
    {
        private IRepository repository;
        private IEmailService mail;

        public AuthenticationService(IRepository repository)
        {
            this.repository = repository;

            var options = MailKitHelper.GetMailKitOptions();
            var provider = new MailKitProvider(options);
            mail = new EmailService(provider);
        }

        public object Login(UserCredentials uc)
        {
            if (string.IsNullOrEmpty(uc.EMail) || string.IsNullOrEmpty(uc.Password))
                throw new Exception("Provide email and password!");

            var user = repository.Users.ByEmailAndPassword(uc.EMail, uc.Password);
            if (user == null)
                return null;

            user.PasswordHash = null;
            var jwt = new JwtToken(repository);
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

            var obj = repository.Users.ByEmail(email);
            if (obj == null)
                throw new Exception("User not found!");

            this.mail.Send(email, "Passwort zurücksetzen", "Bla", true);
        }
    }
}
