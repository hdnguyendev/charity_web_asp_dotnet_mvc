using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DonationsWeb.Data;
using DonationsWeb.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Scripting;
using DonationsWeb.Filter;

namespace DonationsWeb.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly DonationsWebContext _context;

        public UsersController(DonationsWebContext context)
        {
            _context = context;


        }


        // Check if user is logged in
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetString("UserId") != null;
        }

       

        // GET: Users/Login

        public IActionResult Login()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Users/Register

        public IActionResult Register()
        {

            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        // POST: Users/Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var userDb = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (userDb == null)
            {
                ModelState.AddModelError("Email", "Invalid email or password");
                return View(userDb);
            }


            // Save user in session
            HttpContext.Session.SetString("UserId", userDb.UserId.ToString());
            HttpContext.Session.SetString("UserName", userDb.Name);
            HttpContext.Session.SetString("UserEmail", userDb.Email);
            HttpContext.Session.SetString("RoleName", userDb.Role.RoleName);


            return RedirectToAction("Index", "Home");

        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                // Thêm lỗi vào ModelState
                ModelState.AddModelError("Email", "Email already exists.");
            }

            if (ModelState.IsValid)
            {
                var donorRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Donor");

                if (donorRole != null)
                {
                    user.Role= donorRole;
                    // Add user to the database
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    // Redirect to login or another page
                    // Save user in session
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("RoleName", user.Role.RoleName);


                    return RedirectToAction("Index", "Home");
                }

            }

            // Nếu có lỗi, trả lại form kèm thông báo lỗi
            return View(user);

        }


        // GET: Users/Logout

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        // GET: Users
        [AdminAuthorizationFilter]
        public async Task<IActionResult> Index()
        {

            var donationsWebContext = _context.Users.Include(u => u.Role);
            return View(await donationsWebContext.ToListAsync());
        }

        // GET: Users/Details/5
        [AdminAuthorizationFilter]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [AdminAuthorizationFilter]
        public IActionResult Create()
        {
            var roles = _context.Roles
            .Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.RoleName
            })
            .ToList();

            ViewBag.RoleId = roles;

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorizationFilter]
        public async Task<IActionResult> Create([Bind("UserId,Name,Email,Password,PhoneNumber,Address,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Reload roles if model is invalid
            var roles = _context.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                })
                .ToList();

            ViewBag.RoleId = roles;
            return View(user);
        }

        // GET: Users/Edit/5
        [AdminAuthorizationFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = _context.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                })
                .ToList();

            ViewBag.RoleId = roles;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorizationFilter]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Email,Password,PhoneNumber,Address,RoleId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var roles = _context.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                })
                .ToList();

            ViewBag.RoleId = roles;
            return View(user);
        }

        // GET: Users/Delete/5
        [AdminAuthorizationFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [AdminAuthorizationFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
