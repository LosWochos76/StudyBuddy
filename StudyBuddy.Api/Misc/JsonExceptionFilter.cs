using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudyBuddy.Api
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var result = new ObjectResult(new
            {
                code = 500,
                message = "An error occured!",
                detailedMessage = context.Exception.Message
            });

            result.StatusCode = 500;
            context.Result = result;
        }
    }
}
