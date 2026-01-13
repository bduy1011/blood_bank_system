using BB.CR.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BB.CR.Rest
{
    public static class Helper
    {
        public static string GenerateToken(SystemUser? systemUser,
            IConfiguration configuration)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
#pragma warning restore CS8604 // Possible null reference argument.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, systemUser?.UserCode ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),

                new Claim(JwtRegisteredClaimNames.Sid, systemUser?.IdCardNr ?? string.Empty)
            ];

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"],
                claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
