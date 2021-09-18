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
                error = context.Exception.Message,
                source = context.Exception.Source
            });

            result.StatusCode = 500;
            context.Result = result;
        }
    }
}
