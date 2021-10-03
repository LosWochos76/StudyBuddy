﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StudyBuddy.Model;
using StudyBuddy.Persistence;
using Environment = StudyBuddy.Model.Environment;

namespace StudyBuddy.BusinessLogic
{
    public class JwtToken
    {
        private readonly byte[] key;
        private readonly IRepository repository;
        private readonly JwtSecurityTokenHandler tokenHandler = new();

        public JwtToken(IRepository repository)
        {
            this.repository = repository;

            var key_string = Environment.GetOrDefault("JWT_KEY", "thisisasupersecretkey");
            key = Encoding.ASCII.GetBytes(key_string);
        }

        public string FromUser(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.Subject = new ClaimsIdentity(new[] {new Claim("id", user.ID.ToString())});
            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(7);
            tokenDescriptor.SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User FromToken(string token)
        {
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                var user_id = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                return repository.Users.ById(user_id);
            }
            catch
            {
                return null;
            }
        }
    }
}