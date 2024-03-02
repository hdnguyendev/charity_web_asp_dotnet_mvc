using Microsoft.AspNetCore.Mvc;

namespace DonationsWeb.Controllers
{
    public class DonationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
