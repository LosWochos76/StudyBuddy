using Microsoft.AspNetCore.Builder;

namespace StudyBuddy.Services
{
    public static class CustomAuthorizationMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthorizationMiddleware>();
        }
    }
}