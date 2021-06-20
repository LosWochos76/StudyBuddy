using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.Admin
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate next;
        private IRepository repository;

        public CustomAuthorizationMiddleware(RequestDelegate next, IRepository repository)
        {
            this.next = next;
            this.repository = repository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Simulate, that a user is logged in. Remove before check-in
            context.Session.SetInt32("user_id", 1);

            if (!context.Request.Path.StartsWithSegments("/Authentication/Login"))
            {
                if (context.Session.GetInt32("user_id").HasValue)
                {
                    if (context.Items["user"] == null)
                    {
                        var user_id = context.Session.GetInt32("user_id").Value;
                        var user = repository.Users.ById(user_id);
                        context.Items["user"] = user;
                    }
                }
                else
                {
                    context.Response.Redirect("/Authentication/Login");
                }
            }

            await next(context);
        }
    }
}