using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudyBuddy.Model;
using System;
using System.Security.Claims;
using System.Text;
using StudyBuddy.Persistence;
using System.IdentityModel.Tokens.Jwt;
using NETCore.MailKit.Core;

namespace StudyBuddy.Api
{
    public class LoginController : Controller
    {
        private IRepository repository;
        private readonly IEmailService emailservice;

        public LoginController(IRepository repository, IEmailService emailservice)
        {
            this.repository = repository;
            this.emailservice = emailservice;
        }

        [Route("/Login/")]
        [HttpPost]
        public IActionResult Index([FromBody] UserCredentials uc)
        {
            if (string.IsNullOrEmpty(uc.EMail) || string.IsNullOrEmpty(uc.Password))
                return Json(new { Error = "Provide email and password!" });

            var user = repository.Users.ByEmailAndPassword(uc.EMail, uc.Password);
            if (user == null)
            {
                return Json(new { Error = "No user found!" });
            }
            else
            {
                user.PasswordHash = string.Empty;
                return Json(new
                {
                    Token = generateJwtToken(user),
                    User = user
                });
            }
        }

        [Route("/Login/SendPasswortResetMail/")]
        [HttpPost]
        public IActionResult SendPasswortResetMail([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return Json(new { Error = "Provide email and password!" });

            this.emailservice.Send(email, "Passwort zurücksetzen", "Bla", true);

            return Json(new { Status = "ok" });
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Helper.GetFromEnvironmentOrDefault("JWT_KEY", "thisisasupersecretkey"));
            var tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.Subject = new ClaimsIdentity(new[] { new Claim("id", user.ID.ToString()) });
            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(7);
            tokenDescriptor.SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}