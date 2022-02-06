using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using SimpleHashing.Net;

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

        public object Login(UserCredentials uc)
        {
            if (uc == null || string.IsNullOrEmpty(uc.EMail) || string.IsNullOrEmpty(uc.Password))
                throw new Exception("Missing email and password!");

            var user = backend.Repository.Users.ByEmail(uc.EMail);
            if (user == null)
            {
                backend.Logging.LogDebug($"User with email {uc.EMail} not found!");
                return null;
            }

            if (!simpleHash.Verify(uc.Password, user.PasswordHash))
            {
                backend.Logging.LogDebug($"Wrong password for {uc.EMail}!");
                return null;
            }

            backend.Logging.LogDebug("Successfull login");
            user.PasswordHash = null;
            var jwt = new JwtToken();

            return new
            {
                Token = jwt.FromUser(user.ID),
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
            var key = obj.PasswordHash;
            var baseurl = "https://backend.gameucation.eu/login/resetpassword";
            var jwt = new JwtToken();
            var token = jwt.PasswordResetToken(obj.ID, key);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", email }
            };
            var link = new Uri(QueryHelpers.AddQueryString(baseurl, param));

            MailKitHelper.SendMail(email, "Passwort zurücksetzen", link.ToString());
        }

        public bool CheckToken(string token)
        {
            var jwt = new JwtToken();
            return jwt.FromToken(token) != 0;
        }
        public bool CheckPasswordResetToken(string token, string email)
        {
            var jwt = new JwtToken();
            return jwt.CheckPasswordResetToken(token, email);
        }
    }
}