using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudyBuddy.BusinessLogic;

namespace StudyBuddy.Api
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        private IBackend backend;

        public JsonExceptionFilter(IBackend backend)
        {
            this.backend = backend;
        }

        public void OnException(ExceptionContext context)
        {
            backend.Logging.LogError("Exception: " + context.Exception.Message);

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