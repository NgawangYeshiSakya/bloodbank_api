using BBank.Repositories.Contracts;

namespace BBank
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            var path = context.Request.Path.Value;

            // Bypass the user key check for the registration route
            if (!path.StartsWith("/api/Users/register", StringComparison.OrdinalIgnoreCase))
            {
                var userKey = context.Request.Headers["User-Key"].FirstOrDefault();
                if (!string.IsNullOrEmpty(userKey) && await userRepository.ValidateUserKeyAsync(userKey))
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid user key.");
                return;
            }

            await _next(context);
        }
    }

    // Extension method for adding the middleware
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
