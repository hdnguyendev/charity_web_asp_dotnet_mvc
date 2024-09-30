using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DonationsWeb.Data;
using DonationsWeb.Models;
using DonationsWeb.Filter;

namespace DonationsWeb.Controllers
{
    public class DonationsController : Controller
    {
        private readonly DonationsWebContext _context;

        public DonationsController(DonationsWebContext context)
        {
            _context = context;
        }

        // GET: Donations
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Index()
        {
            var donationsWebContext = _context.Donations.Include(d => d.Campaign).Include(d => d.User);
            return View(await donationsWebContext.ToListAsync());
        }

        // GET: Donations/Details/5
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.Campaign)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/Create
        [AdminAuthorizationFilter]

        public IActionResult Create()
        {
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "CampaignId", "CampaignId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Donations/Donate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Donate(int UserId, int CampaignId, float Amount, string Message)
        {
            // check if user exists
            if (!_context.Users.Any(u => u.UserId == UserId))
            {
                return Json(new { success = false, message = "User not found" });
            }

            // check if campaign exists
            if (!_context.Campaigns.Any(c => c.CampaignId == CampaignId))
            {
                return Json(new { success = false, message = "Campaign not found" });

            }




            // save donate
            Donation donation = new Donation
            {
                UserId = UserId,
                CampaignId = CampaignId,
                Amount = Amount,
                Message = Message,
                DonationDate = DateTime.Now
            };

            _context.Donations.Add(donation);
            // update campaign amount
            var campaign = _context.Campaigns.Find(CampaignId);
            if (campaign != null)
            {
                campaign.CurrentAmount += Amount; // Cập nhật số tiền quyên góp
                _context.Campaigns.Update(campaign);
            }
            _context.SaveChanges();
            return Json(new { success = true, message = "Donation successful!" });

        }

        // POST: Donations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Create([Bind("DonationId,UserId,CampaignId,Amount,DonationDate,Message")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "CampaignId", "CampaignId", donation.CampaignId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", donation.UserId);
            return View(donation);
        }

        // GET: Donations/Edit/5
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "CampaignId", "CampaignId", donation.CampaignId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", donation.UserId);
            return View(donation);
        }

        // POST: Donations/Edit/5
        [AdminAuthorizationFilter]

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DonationId,UserId,CampaignId,Amount,DonationDate,Message")] Donation donation)
        {
            if (id != donation.DonationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.DonationId))
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
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "CampaignId", "CampaignId", donation.CampaignId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", donation.UserId);
            return View(donation);
        }

        // GET: Donations/Delete/5
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.Campaign)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DonationId == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [AdminAuthorizationFilter]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.DonationId == id);
        }
    }
}
