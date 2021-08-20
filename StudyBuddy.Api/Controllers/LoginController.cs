using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudyBuddy.Model;
using System;
using System.Security.Claims;
using System.Text;
using StudyBuddy.Persistence;
using System.IdentityModel.Tokens.Jwt;

namespace StudyBuddy.Services
{
    public class LoginController : Controller
    {
        private IRepository repository;

        public LoginController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("/Login/")]
        [HttpPost]
        public IActionResult Index([FromBody] UserCredentials uc)
        {
            if (string.IsNullOrEmpty(uc.EMail) || string.IsNullOrEmpty(uc.Password))
                return Json(new { Error = "Provide email and password!"});

            var user = repository.Users.ByEmailAndPassword(uc);
            if (user == null)
            {
                return Json(new { Error = "No user found!"});
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