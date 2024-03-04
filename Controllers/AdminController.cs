using Microsoft.AspNetCore.Mvc;

namespace DonationsWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
