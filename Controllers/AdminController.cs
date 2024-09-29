using DonationsWeb.Filter;
using Microsoft.AspNetCore.Mvc;

namespace DonationsWeb.Controllers
{
    [AdminAuthorizationFilter]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
