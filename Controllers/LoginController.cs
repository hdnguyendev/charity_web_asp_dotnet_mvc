using Microsoft.AspNetCore.Mvc;

namespace DonationsWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
