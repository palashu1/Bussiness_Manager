using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Bussiness_Manager.Utility
{
    public static class Helper
    {
        public static string GetClaimsFromToken(HttpContext httpContext, IConfiguration _configuration)
        {
            var token = httpContext.Request.Cookies["AuthToken"];
            var tokenHandler = new JwtSecurityTokenHandler();
            
            // Validate the token and extract the claims
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            // Extract claims
            var claims = principal.Claims;
            var memberId = claims.Where(w => w.Type == "memberId").FirstOrDefault();
            return memberId.Value;
        }

        public enum loginType
        {
            firstToken=1,
            secondToken=2
        };
    }
}
