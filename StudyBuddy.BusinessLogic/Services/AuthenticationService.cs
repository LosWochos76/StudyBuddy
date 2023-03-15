using Microsoft.AspNetCore.WebUtilities;
using SimpleHashing.Net;
using SkiaSharp;
using StudyBuddy.Model;
using StudyBuddy.Model.Enum;
using System;
using System.Collections.Generic;

namespace StudyBuddy.BusinessLogic
{
    class AuthenticationService : IAuthenticationService
    {
        private SimpleHash simpleHash = new SimpleHash();
        private readonly IBackend backend;

        public AuthenticationService(IBackend backend)
        {
            this.backend = backend;
        }

        public LoginResult Login(UserCredentials uc)
        {
            if (uc == null || string.IsNullOrEmpty(uc.EMail) || string.IsNullOrEmpty(uc.Password))
                throw new Exception("Missing email and password!");

            var user = backend.Repository.Users.ByEmailAllAccounts(uc.EMail);
            if (user == null)
            {
                backend.Logging.LogDebug($"User with email {uc.EMail} not found!");
                return new LoginResult() { Status = LoginStatus.UserNotFound };
            }

            if (!user.AccountActive)
            {
                backend.Logging.LogDebug($"User with email {uc.EMail} is disabled.");
                return new LoginResult() { Status = LoginStatus.AccountDisabled };
            }

            if (!simpleHash.Verify(uc.Password, user.PasswordHash))
            {
                backend.Logging.LogDebug($"Wrong password for {uc.EMail}!");
                return new LoginResult() { Status = LoginStatus.IncorrectCredentials };
            }

            if (!user.EmailConfirmed)
            {
                backend.Logging.LogDebug($"User with email {user.Email} is not verified.");
                return new LoginResult() { Status = LoginStatus.EmailNotVerified };
            }

            backend.Logging.LogDebug("Successfull login");
            user.PasswordHash = null;
            var jwt = new JwtToken();
            return new LoginResult(jwt.FromUser(user.ID), user) { Status = LoginStatus.Success };
        }

        public bool CheckForValidMail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            if (backend.Repository.Users.ByEmailActiveAccounts(email) is null)
                return false;

            return true;
        }

        public string GenerateUserToken(string email)
        {
            var obj = backend.Repository.Users.ByEmailActiveAccounts(email);
            var jwt = new JwtToken();
            return jwt.PasswordResetToken(obj.ID, obj.PasswordHash);
        }

        public void SendVerificationMail(string email)
        {
            if (!CheckForValidMail(email))
                return;

            string baseurl = Settings.BackendUrl + "/login/verifyemail";
            string subject = "E-Mail Adresse bestätigen";
            var param = new Dictionary<string, string>
            {
                { "token", GenerateUserToken(email) },
                { "email", email }
            };

            Uri link = new Uri(QueryHelpers.AddQueryString(baseurl, param));
            string message = "Guten Tag,<br> um die E-Mail-Adresse Ihres Gameucation Kontos zu bestätigen <a href='" + link.ToString() + "'>hier</a> klicken.";
            MailKitHelper.SendMailAsync(email, subject, message);
        }

        public void SendPasswordResetMail(string email)
        {
            if (!CheckForValidMail(email))
                return;

            string baseurl = Settings.ApiUrl + "/login/generatepassword";
            string subject = "Passwort zurücksetzen";
            var param = new Dictionary<string, string>
            {
                { "token", GenerateUserToken(email) },
                { "email", email }
            };

            Uri link = new Uri(QueryHelpers.AddQueryString(baseurl, param));
            string message = "Guten Tag,<br> um das Passwort Ihres Gameucation Kontos zurückzusetzen <a href='" + link.ToString() + "'>hier</a> klicken.";
            MailKitHelper.SendMailAsync(email, subject, message);
        }

        public bool CheckToken(string token)
        {
            var jwt = new JwtToken();
            return jwt.FromToken(token) != 0;
        }

        public bool CheckPasswordResetToken(string token, string hash)
        {
            var jwt = new JwtToken();
            return jwt.CheckPasswordResetToken(token, hash);
        }
    }
}