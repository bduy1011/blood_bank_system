using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BB.CR.Rest.Bases
{
    [ApiController]
    [Route("api/base")]
    public class BaseController(IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

        protected string? GetIdentityCard()
        {
            if (_contextAccessor?.HttpContext?.User?.Claims is not { } claims)
                return null;

            var c = claims.FirstOrDefault(i => string.Equals(i.Type, System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid, StringComparison.OrdinalIgnoreCase));
            return c?.Value;
        }

        protected string? GetUserCode()
        {
            if (_contextAccessor?.HttpContext?.User?.Claims is not { } claims)
                return null;

            // ASP.NET Core JWT mặc định map claim "sub" sang ClaimTypes.NameIdentifier
            var c = claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)
                 ?? claims.FirstOrDefault(i => string.Equals(i.Type, System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, StringComparison.OrdinalIgnoreCase));
            return c?.Value;
        }

        protected string? GetFirebaseToken() 
        {
            return _contextAccessor?.HttpContext?.Request?.Headers["FireBaseToken"];
        }

        protected string? GetDevice() 
        {
            return _contextAccessor?.HttpContext?.Request?.Headers["DeviceId"];
        }
    }
}
