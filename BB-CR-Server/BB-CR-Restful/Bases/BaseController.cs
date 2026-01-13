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
            if (_contextAccessor is null)
                return null;

            var identityCard = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(i => i.Type.Equals(nameof(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid), StringComparison.OrdinalIgnoreCase));
            if (identityCard is null)
                return null;

            return identityCard.Value;
        }

        protected string? GetUserCode()
        {
            if (_contextAccessor is null)
                return null;

            var userCode = _contextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(i => i.Type.Equals(nameof(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub), StringComparison.OrdinalIgnoreCase));
            if (userCode is null)
                return null;

            return userCode.Value;
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
