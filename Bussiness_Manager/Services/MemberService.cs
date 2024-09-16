using Bussiness_Manager.Dto;
using Bussiness_Manager.Models;
using Bussiness_Manager.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Bussiness_Manager.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContex _context;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MemberService(ApplicationDbContex _dbcontext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._context = _dbcontext;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IGenericContainer<string>> createMember(membersDto Dto)
        {
            GenericContainer<string> result = new GenericContainer<string>();
            int save = 0;
            try
            {
                var isMobileNoExit = await _context.members.AnyAsync(x => x.phone == Dto.phone);
                if (isMobileNoExit)
                {
                    result.status = "Mobile no already exit.";
                    result.Value = "false";
                    return result;
                }

                Members members = new Members()
                {
                    firstName = Dto.firstName,
                    lastName = Dto.lastName,
                    phone = Dto.phone,
                    email = Dto.email,
                    password = PasswordHelper.EncryptPassword(Dto.password),
                    dstatus = "V",
                    createdOn = indiaTimeZone.DateTimeIndia(),
                    updatedOn = indiaTimeZone.DateTimeIndia(),
                };
                await _context.members.AddAsync(members);
                save = await _context.SaveChangesAsync();
                if (save > 0)
                {
                    result.status = "success";
                }
                else
                {
                    result.status = "Sign up failed.";
                }

            }
            catch (Exception ex)
            {
                result.status = "Server error";
            }

            return result;
        }

        public async Task<IGenericContainer<string>> login(loginDto dto)
        {
            GenericContainer<string> result = new GenericContainer<string>();
            try
            {
                var member = await _context.members.Where(w => w.phone == dto.phone && w.dstatus == "V").FirstOrDefaultAsync();
                if (member != null)
                {
                    bool isPasswordCorrect = PasswordHelper.DycryptPassword(member.password, dto.password);
                    if (isPasswordCorrect)
                    {
                        //Generate Jwt token
                        string token = generateJwtToken(member.memberId);
                        if (token != null)
                        {
                            bool isShopExist = await _context.shopDetails.Where(w => w.memberId == member.memberId && w.dstatus == "V").AnyAsync();
                            if (isShopExist)
                            {
                                result.status = "Successfully Login";
                            }
                            else
                            {
                                result.status = "Successfully_Login_with_shop";
                            }
                            result.Value = member.memberId.ToString();
                        }
                        else
                        {
                            result.status = "Token Generation Problem! Login again";
                            result.Value = "";
                        }
                    }
                    else
                    {
                        result.status = "Invalid username or password";
                        result.Value = "";
                    }
                }
                else
                {
                    result.status = "Invalid username or password";
                    result.Value = "";
                }
            }
            catch (Exception ex)
            {
                result.status = "Server error";
                result.Value = "";
            }
            return result;
        }

        public async Task<IGenericContainer<List<shopDetailDto>>> getAllShops(int memberId)
        {
            GenericContainer<List<shopDetailDto>> result = new GenericContainer<List<shopDetailDto>>();
            try
            {
                var shopDetailDto = await _context.shopDetails.Where(w => w.memberId == memberId && w.dstatus == "V").Select(s => new shopDetailDto()
                {
                    shopId = s.shopId,
                    shopName = s.shopName,
                    shopDescription = s.shopDescription,
                    shopAddress = s.shopAddress,
                    logo = s.logo,
                    bussinessType = s.bussinessType,
                    dstatus = s.dstatus,
                    createdOn = s.createdOn,
                    updatedOn = s.updatedOn,
                }).OrderByDescending(o => o.updatedOn).ToListAsync();
                if (shopDetailDto != null && shopDetailDto.Count != 0)
                {
                    result.status = "list";
                    result.Value = shopDetailDto;
                }
                else
                {
                    result.status = "First Add Shop";
                    result.Value = shopDetailDto;
                }
            }
            catch (Exception ex)
            {
                result.status = "Server error";
            }
            return result;
        }

        public async Task<IGenericContainer<string>> resetTokenUpdateCookie(int memberId, int shopId)
        {
            GenericContainer<string> result = new GenericContainer<string>();
            string token= updateJwtToken(memberId, shopId);
            if (string.IsNullOrEmpty(token))
            {
                result.status = "error";
            }
            else {
                result.status = "success";
                result.Value = token;
            }
            return result;
        }

        private string generateJwtToken(int memberId)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("memberId",memberId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(7),
                signingCredentials: signIn
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);


            // set token in cookie

            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Response.Cookies.Append("AuthToken", tokenValue, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                });
            }

            return tokenValue;
        }

        private string updateJwtToken(int memberId, int shopId)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("memberId",memberId.ToString()),
                new Claim("shopId",shopId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(7),
                signingCredentials: signIn
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);


            // set token in cookie

            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Response.Cookies.Append("AuthToken", tokenValue, new CookieOptions
                {
                    //HttpOnly = true,
                    //Secure = true,
                    //Expires = DateTimeOffset.UtcNow.AddHours(1)
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                });
            }

            return tokenValue;
        }
    }
}
