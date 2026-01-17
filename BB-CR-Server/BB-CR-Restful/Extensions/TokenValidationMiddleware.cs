using BB.CR.Rest.Services;

namespace BB.CR.Rest.Extensions
{
    public class TokenValidationMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        private readonly RequestDelegate _next = next;
        private readonly ITokenService _tokenService = tokenService;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Allow refresh-token endpoint to proceed even if token was revoked.
            // This enables exchanging a recently-revoked token for a new one (biometric flow).
            if (httpContext.Request.Path.Value?.Contains("refresh-token") == true)
            {
                await _next(httpContext);
                return;
            }

            var token = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!string.IsNullOrWhiteSpace(token)
                && _tokenService.IsTokenRevoked(token))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Token has been revoked.");
                return;
            }

            await _next(httpContext);
        }
    }
}
