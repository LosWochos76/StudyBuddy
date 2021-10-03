using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudyBuddy.BusinessLogic;
using StudyBuddy.Persistence;

namespace StudyBuddy.Api
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JwtToken jwt;

        public CustomAuthorizationMiddleware(RequestDelegate next, IRepository repository)
        {
            this.next = next;
            jwt = new JwtToken(repository);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                var user = jwt.FromToken(token);
                context.Items["user"] = user;
            }

            await next(context);
        }
    }
}