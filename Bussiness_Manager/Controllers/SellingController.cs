using Bussiness_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bussiness_Manager.Controllers
{
    [Authorize]
    public class SellingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> addItem(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addItem()
        {
            return View();
        }


        public async Task<IActionResult> addCustomer(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addCustomer(Customer customer)
        {

            return View();
        }


    }
}
