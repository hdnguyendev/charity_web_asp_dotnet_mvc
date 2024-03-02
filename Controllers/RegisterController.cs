using Microsoft.AspNetCore.Mvc;

namespace DonationsWeb.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
