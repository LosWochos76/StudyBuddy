using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace StudyBuddy.BusinessLogic
{
    public class JwtToken
    {
        private readonly byte[] key;
        private readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private readonly string domain_name;

        public JwtToken()
        {
            var key_string = Model.Environment.GetOrDefault("JWT_KEY", "thisisasupersecretkey");
            domain_name = Model.Environment.GetOrDefault("DOMAIN_NAME", "localhost");
            key = Encoding.ASCII.GetBytes(key_string);
        }

        public string FromUser(int user_id)
        {
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user_id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = domain_name
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int FromToken(string token)
        {
            try
            {
                var tp = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = domain_name
                };

                tokenHandler.ValidateToken(token, tp, out var validatedToken);
                var jwtToken = (JwtSecurityToken) validatedToken;
                var user_id = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                return user_id;
            }
            catch
            {
                return 0;
            }
        }
    }
}