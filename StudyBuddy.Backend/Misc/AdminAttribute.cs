using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudyBuddy.Model;

namespace StudyBuddy.Admin
{
    public class AdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.Items["user"] as User;
            if (user != null && !user.IsAdmin)
                filterContext.Result = new RedirectResult("/Challenge");
        }
    }
}