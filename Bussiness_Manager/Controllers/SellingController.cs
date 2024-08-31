using Microsoft.AspNetCore.Mvc;

namespace Bussiness_Manager.Controllers
{
    public class SellingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
