using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudyBuddy.Model;

namespace StudyBuddy.Api
{
    public class IsAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items["user"] == null)
            {
                context.Result = new JsonResult(new { Error = "Unauthorized" });
            }
            else
            {
                var user = context.HttpContext.Items["user"] as User;
                if (!user.IsAdmin)
                    context.Result = new JsonResult(new { Error = "Unauthorized" });
            }
        }
    }
}