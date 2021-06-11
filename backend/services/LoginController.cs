using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudyBuddy.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;

namespace StudyBuddy.Services
{
    public class LoginController : Controller
    {
        private StudyBuddyContext context;
        private ISimpleHash simpleHash = new SimpleHash();

        public LoginController(StudyBuddyContext context)
        {
            this.context = context;
        }

        [Route("/Login/")]
        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Json(new { Error = "Provide email and password!"});

            var list = (from obj in context.Users 
                where obj.Email.Equals(email) select obj).AsNoTracking().ToList();

            foreach (var obj in list)
                if (simpleHash.Verify(password, obj.PasswordHash))
                    return Json(new { Token = generateJwtToken(obj) });
            
            return Json(new { Error = "No user found!"});
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