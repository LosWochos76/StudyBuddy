using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IBackend backend)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            backend.SetCurrentUserFromToken(token);
            await next(context);
        }
    }
}