using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DonationsWeb.Data;
using DonationsWeb.Models;
using Microsoft.IdentityModel.Tokens;

namespace DonationsWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly DonationsWebContext _context;

        public UsersController(DonationsWebContext context)
        {
            _context = context;
        }
        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }
        // GET: Users/Login
        public IActionResult Register()
        {
            return View();
        }
        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Kiểm tra thông tin đăng nhập
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // Email không tồn tại, hiển thị thông báo lỗi
                ModelState.AddModelError("Email", "Email này chưa đăng ký tài khoản trên hệ thống.");
                return View(user);
            }

            if (user.Password != password)
            {
                // Mật khẩu không chính xác, hiển thị thông báo lỗi
                ModelState.AddModelError("Password", "Mật khẩu không chính xác.");
                return View(user);
            }
            // Lưu thông tin người dùng đã đăng nhập vào session
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.Name.ToString());
            HttpContext.Session.SetString("UserEmail", user.Email.ToString());
            HttpContext.Session.SetString("Role", user.Role.ToString());



            // Điều hướng tới action hoặc trang chính sau khi đăng nhập thành công
            return RedirectToAction("Index", "Home");
        }
        // GET: Customers/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Email,Password,PhoneNumber,Address")] User user, string role)
        {

            // Kiểm tra xem email đã tồn tại trong hệ thống hay chưa
            bool emailExists = await _context.User.AnyAsync(u => u.Email == user.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống.");
                // Trả về view với lỗi
                return View(user);
            }
            if (role == "Contributter")
            {
                user.Role = 1;
            }
            else if (role == "Organization") 
            {
                user.Role = 2;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
            }
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }
        // GET: Users
        public async Task<IActionResult> Index()

        {
            string? role = HttpContext.Session.GetString("Role");
            if (role.IsNullOrEmpty() || role.Equals("1") || role.Equals("2"))
            {
                return RedirectToAction(nameof(Login));
            }
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            string? role = HttpContext.Session.GetString("Role");
            if (role.IsNullOrEmpty() || role.Equals("1") || role.Equals("2"))
            {
                return RedirectToAction(nameof(Login));
            }
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Email,Password,PhoneNumber,Address,Role")] User user)
        {
            string? role = HttpContext.Session.GetString("Role");
            if (role.IsNullOrEmpty() || role.Equals("1") || role.Equals("2"))
            {
                return RedirectToAction(nameof(Login));
            }
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string? role = HttpContext.Session.GetString("Role");
            if (role.IsNullOrEmpty() || role.Equals("1") || role.Equals("2"))
            {
                return RedirectToAction(nameof(Login));
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Email,Password,PhoneNumber,Address,Role")] User user)
        {
            string? role = HttpContext.Session.GetString("Role");
            if (role.IsNullOrEmpty() || role.Equals("1") || role.Equals("2"))
            {
                return RedirectToAction(nameof(Login));
            }
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string? role = HttpContext.Session.GetString("Role");
            if (role.IsNullOrEmpty() || role.Equals("1") || role.Equals("2"))
            {
                return RedirectToAction(nameof(Login));
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? role = HttpContext.Session.GetString("Role");
            if (role.IsNullOrEmpty() || role.Equals("1") || role.Equals("2"))
            {
                return RedirectToAction(nameof(Login));
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
