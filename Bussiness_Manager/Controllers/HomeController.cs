using Bussiness_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bussiness_Manager.Utility;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Bussiness_Manager.Services;
using Bussiness_Manager.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Bussiness_Manager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContex _context;
        private readonly IMemberService _memberService;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContex _Dbcontex,IMemberService memberService, IConfiguration configuration)
        {
            _logger = logger;
            _context = _Dbcontex;
            _memberService = memberService;
            _configuration = configuration;
        }


        [AllowAnonymous]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Registration(Members members)
        {
            IGenericContainer<string> result = new GenericContainer<string>();
            if (ModelState.IsValid)
            {
                membersDto Dto = new membersDto()
                {
                    memberId = members.memberId,
                    firstName = members.firstName,
                    lastName = members.lastName,
                    email = members.email,
                    password = members.password,
                    phone = members.phone,
                    confirmPassword = members.confirmPassword,
                };

                result = await _memberService.createMember(Dto);
                if (result.status == "success")
                {
                    TempData["ShowAlert"] = true;
                    ViewBag.message = "Sign Up Successfully";
                    return View();
                }

                TempData["ShowAlert1"] = true;
                ViewBag.message = result.status;
                return View();
            }
            
           return View(members);
        }

        public async Task<IActionResult>getShops()
        {
            IGenericContainer<List<shopDetailDto>> result = new GenericContainer<List<shopDetailDto>>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            result = await _memberService.getAllShops(Convert.ToInt32(memberId));
            var shops=result.Value.Select(s=>new ShopDetail()
            {
                shopId=(int)s.shopId,
                shopName=s.shopName,
                shopDescription=s.shopDescription,
                shopAddress=s.shopAddress,
                bussinessType=s.bussinessType,
                createdOn=s.createdOn,
            }).ToList();
            if(result.status== "list")
            {
                ViewBag.Shops = result.Value.Count();
                return View(shops);
            }

            TempData["ShowAlert"] = true;
            ViewBag.message = result.status;
            ViewBag.icon = "error";
            ViewBag.title = "Failed";

            return View(shops);
        }

        [HttpPost]
        public async Task<IActionResult> updateToken(int selectedCheckbox)
        {
            IGenericContainer<string> result = new GenericContainer<string>();
            var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            result = await _memberService.resetTokenUpdateCookie(Convert.ToInt32(memberId), selectedCheckbox);
            return RedirectToAction("Index", "Selling");
        }

        public IActionResult AddShop()
        {
            ShopDetail shopDetail=new ShopDetail();
            int memberId= Convert.ToInt32(TempData["Data"]);
            var shoData = _context.shopDetails.Where(w => w.memberId == memberId && w.dstatus == "V").FirstOrDefault();
            if(shoData == null)
            {
                shopDetail.shopId = 0;
                shopDetail.memberId= memberId;
            }
            else
            {
                shopDetail.shopId = 1;
                shopDetail.memberId = memberId;
            }
            return View(shopDetail);
        }

        [HttpPost]
        public async Task<IActionResult> AddShop(ShopDetail shopDetail)
        {
             var memberId = Helper.GetClaimsFromToken(HttpContext, _configuration);
            if (ModelState.IsValid)
            {
                shopDetail.memberId = Convert.ToInt32(memberId);
                shopDetail.dstatus = "V";
                shopDetail.createdOn= indiaTimeZone.DateTimeIndia();
                shopDetail.updatedOn= indiaTimeZone.DateTimeIndia();
                shopDetail.logo = "Logo";
                await _context.shopDetails.AddAsync(shopDetail);
                await _context.SaveChangesAsync();
                TempData["IsAddShop"] = "Add.";
                return RedirectToAction("getShops");
            }
            return View(shopDetail);
        }

        [AllowAnonymous]
        public IActionResult Login() 
        {
            return View();
        }

        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Login(loginModel model)
        {
            IGenericContainer<string> result = new GenericContainer<string>();
        
            if (ModelState.IsValid)
            {
                loginDto dto = new loginDto()
                {
                    phone = model.phone,
                    password = model.password,
                };
                result = await _memberService.login(dto);
  
                if (result.status == "Successfully Login")
                {
                    TempData["ShowAlert1"] = true;
                    ViewBag.message = result.status;
                    ViewBag.icon = "success";
                    ViewBag.title = "Done";
                    TempData["UserId"] = result.Value;
                    return View();
                }
                else if(result.status == "Successfully_Login_with_shop")
                {
                    TempData["ShowAlert2"] = true;
                    ViewBag.message = "Successfully Login";
                    ViewBag.icon = "success";
                    ViewBag.title = "Done";
                    TempData["UserId"] = result.Value;
                    return View();
                }

                TempData["ShowAlert"] = true;
                ViewBag.message = result.status;
                ViewBag.icon = "error";
                ViewBag.title = "Failed";
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Remove the JWT token by clearing the cookie
            if (HttpContext.Request.Cookies["AuthToken"] != null)
            {
                // Delete the "AuthToken" cookie
                HttpContext.Response.Cookies.Delete("AuthToken");
            }

            // Optionally, clear the user's session data
            HttpContext.Session.Clear();

            // Redirect to the login page or any other page
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Dashbord()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Dashbord(int id)
        {
            return View();
        }








        #region::old
        public IActionResult Privacy()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
