using BB.CR.Rest.Services;

namespace BB.CR.Rest.Extensions
{
    public class TokenValidationMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        private readonly RequestDelegate _next = next;
        private readonly ITokenService _tokenService = tokenService;

        public async Task InvokeAsync(HttpContext httpContext)
        {
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
