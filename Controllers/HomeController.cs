using DonationsWeb.Data;
using DonationsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DonationsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly DonationsWebContext _context;


        public HomeController(DonationsWebContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve 3 campaigns from the database
            var campaigns = _context.Campaigns.Take(3).ToList();

            // Pass the campaigns to the view
            return View(campaigns);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
