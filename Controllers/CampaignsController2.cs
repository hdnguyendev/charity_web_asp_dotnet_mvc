//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using DonationsWeb.Data;
//using DonationsWeb.Models;

//namespace DonationsWeb.Controllers
//{
//    public class CampaignsController2 : Controller
//    {
//        private readonly DonationsWebContext _context;

//        public CampaignsController2(DonationsWebContext context)
//        {
//            _context = context;
//        }

//        // GET: Campaigns
//        public async Task<IActionResult> Index()
//        {
//            var donationsWebContext = _context.Campaigns.Include(c => c.User);
//            return View(await donationsWebContext.ToListAsync());
//        }

//        // GET: Campaigns/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var campaign = await _context.Campaigns
//                .Include(c => c.User)
//                .FirstOrDefaultAsync(m => m.CampaignId == id);
//            if (campaign == null)
//            {
//                return NotFound();
//            }

//            return View(campaign);
//        }

//        // GET: Campaigns/Create
//        public IActionResult Create()
//        {
//            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
//            // SHOW ONLY THE NAMES OF THE USERS HAVE ROLE ADMIN
//            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.Role.RoleName == "Admin"), "UserId", "Name");

//            return View();
//        }

//        // POST: Campaigns/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("CampaignId,Title,Description,StartDate,EndDate,UserId")] Campaign campaign)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(campaign);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }

//            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", campaign.UserId);
//            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.Role.RoleName == "Admin"), "UserId", "Name", campaign.UserId);

//            return View(campaign);
//        }

//        // GET: Campaigns/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var campaign = await _context.Campaigns.FindAsync(id);
//            if (campaign == null)
//            {
//                return NotFound();
//            }

//            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", campaign.UserId);
//            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.Role.RoleName == "Admin"), "UserId", "Name", campaign.UserId);

//            return View(campaign);
//        }

//        // POST: Campaigns/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("CampaignId,Title,Description,StartDate,EndDate,UserId")] Campaign campaign)
//        {
//            if (id != campaign.CampaignId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(campaign);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CampaignExists(campaign.CampaignId))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }

//            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.Role.RoleName == "Admin"), "UserId", "Name", campaign.UserId);

//            return View(campaign);
//        }

//        // GET: Campaigns/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var campaign = await _context.Campaigns
//                .Include(c => c.User)
//                .FirstOrDefaultAsync(m => m.CampaignId == id);
//            if (campaign == null)
//            {
//                return NotFound();
//            }

//            return View(campaign);
//        }

//        // POST: Campaigns/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var campaign = await _context.Campaigns.FindAsync(id);
//            if (campaign != null)
//            {
//                _context.Campaigns.Remove(campaign);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool CampaignExists(int id)
//        {
//            return _context.Campaigns.Any(e => e.CampaignId == id);
//        }
//    }
//}
