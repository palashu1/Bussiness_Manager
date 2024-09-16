using Azure.Core;
using Bussiness_Manager.Dto;
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

        public static string GetShopIdFromToken(HttpContext httpContext, IConfiguration _configuration)
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
            var shopId = claims.Where(w => w.Type == "shopId").FirstOrDefault();
            return shopId.Value;
        }

        public enum loginType
        {
            firstToken=1,
            secondToken=2
        };

        public static string? getPaymentMode(int id)
        {
            var modeOfPayments = new List<modeOfPaymentDto>
            {
                new modeOfPaymentDto{id=1, paymentMode="Bank Transfer"},
                new modeOfPaymentDto{id=2, paymentMode="Cash"},
                new modeOfPaymentDto{id=3, paymentMode="Credit Card"},
                new modeOfPaymentDto{id=4, paymentMode="Cheque"},
                new modeOfPaymentDto{id=4, paymentMode="UPI"}
            };

            string payMode= modeOfPayments.Where(w => w.id == id).Select(s=>s.paymentMode).FirstOrDefault();
            return payMode;
        }
    }
}
