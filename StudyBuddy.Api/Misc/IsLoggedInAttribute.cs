using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudyBuddy.Api
{
    public class IsLoggedInAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items["user"] == null)
                context.Result = new JsonResult(new {Error = "Unauthorized"});
        }
    }
}