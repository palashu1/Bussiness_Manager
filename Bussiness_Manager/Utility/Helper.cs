using Azure.Core;
using Bussiness_Manager.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Bussiness_Manager.Utility
{
    public static class Helper
    {
        private static readonly string[] _units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] _tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
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

        public static string ConvertNumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + ConvertNumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += ConvertNumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertNumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertNumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                    words += _units[number];
                else
                {
                    words += _tens[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + _units[number % 10];
                }
            }

            return words.Trim();
        }

        public static string GetInitials(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return string.Empty;

            var names = fullName.Split(' ');
            string initials = string.Empty;

            if (names.Length > 0 && names[0].Length > 0)
                initials += names[0][0];  // First letter of first name

            if (names.Length > 1 && names[1].Length > 0)
                initials += names[1][0];  // First letter of last name

            return initials.ToUpper();  // Convert to uppercase
        }

    }
}
