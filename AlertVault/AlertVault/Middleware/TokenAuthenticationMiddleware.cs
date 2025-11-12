using AlertVault.Core.Service;

namespace AlertVault.Middleware
{
    public class TokenAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserService userService)
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader["Bearer ".Length..];
                if (!Guid.TryParse(token, out var guidToken))
                {
                    await _next(context);
                    return;
                }
                
                var userToken = await userService.ValidateToken(guidToken);
                if (userToken != null)
                {
                    context.Items["UserToken"] = userToken;
                }
            }
            await _next(context);
        }
    }
}
