using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IBackend backend;

        public CustomAuthorizationMiddleware(RequestDelegate next, IBackend backend)
        {
            this.next = next;
            this.backend = backend;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            backend.SetCurrentUserFromToken(token);
            await next(context);
        }
    }
}