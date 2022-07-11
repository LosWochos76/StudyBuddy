using Microsoft.AspNetCore.WebUtilities;
using SimpleHashing.Net;
using StudyBuddy.Model;
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
                return new LoginResult 
                {
                    Status = 3
                };
            }

            if (!user.AccountActive)
            {
                if (!simpleHash.Verify(uc.Password, user.PasswordHash))
                {
                    backend.Logging.LogDebug($"Wrong password for {uc.EMail}!");
                    return new LoginResult
                    {
                        Status = 2
                    };
                }
                else
                {
                    backend.Logging.LogDebug($"User with email {uc.EMail} is disabled.");
                    return new LoginResult
                    {
                        Status = 7
                    };
                }
            }

            if (!simpleHash.Verify(uc.Password, user.PasswordHash))
            {
                backend.Logging.LogDebug($"Wrong password for {uc.EMail}!");
                return new LoginResult
                {
                    Status = 2
                };
            }

            backend.Logging.LogDebug("Successfull login");
            user.PasswordHash = null;
            var jwt = new JwtToken();

            if (!user.EmailConfirmed)
            {
                backend.Logging.LogDebug($"User with email {user.Email} is not verified.");
                return new LoginResult
                {
                    Status = 1
                };
            }
            else
            {
                return new LoginResult
                {
                    Status = 0,
                    Token = jwt.FromUser(user.ID),
                    User = user
                };
            }
        }

        public void SendMail(string email, bool forgotpassword)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("No email-adress given!");

            var obj = backend.Repository.Users.ByEmailActiveAccounts(email);
            if (obj == null)
                throw new Exception("User not found!");

            var key = obj.PasswordHash;
            var jwt = new JwtToken();
            var token = jwt.PasswordResetToken(obj.ID, key);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", email }
            };

            string baseurl;
            string subject;
            string message;

            if (forgotpassword)
            {
                baseurl = "https://backend.gameucation.eu/login/resetpassword";
                subject = "Passwort zurücksetzen";
                Uri link = new Uri(QueryHelpers.AddQueryString(baseurl, param));
                message = "Guten Tag,<br> um das Passwort Ihres Gameucation Kontos zurückzusetzen <a href='" + link.ToString() + "'>hier</a> klicken.";
            }
            else
            {
                baseurl = "https://backend.gameucation.eu/login/verifyemail";
                subject = "E-Mail Adresse bestätigen";
                Uri link = new Uri(QueryHelpers.AddQueryString(baseurl, param));
                message = "Guten Tag,<br> um die E-Mail-Adresse Ihres Gameucation Kontos zu bestätigen <a href='" + link.ToString() + "'>hier</a> klicken.";
            }

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