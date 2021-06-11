using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StudyBuddy.Model;

namespace StudyBuddy.Services
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, StudyBuddyContext db)
        {
            if (!context.Request.Path.StartsWithSegments("/Login"))
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    SendError(context, "Unauthorized");
                    return;
                }

                var user = GetUserFromToken(db, token);
                if (user == null)
                {
                    SendError(context, "User from JWT-token not found");
                    return;
                }

                context.Items["user"] = user;
            }

            await next(context);
        }

        private void SendError(HttpContext context, string message)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            string json = System.Text.Json.JsonSerializer.Serialize(new { Error=message });
            context.Response.WriteAsync(json, Encoding.UTF8);
        }

        private User GetUserFromToken(StudyBuddyContext db, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Helper.GetFromEnvironmentOrDefault("JWT_KEY", "thisisasupersecretkey"));

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                int user_id = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var user = db.Users.Find(user_id);
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}