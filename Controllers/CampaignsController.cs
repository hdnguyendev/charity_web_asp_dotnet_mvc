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
    public class CampaignsController : Controller
    {
        private readonly DonationsWebContext _context;

        public CampaignsController(DonationsWebContext context)
        {
            _context = context;
        }

        // GET: Campaigns
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Index()
        {
            var donationsWebContext = _context.Campaigns.Include(c => c.User);
            return View(await donationsWebContext.ToListAsync());
        }

        // GET: Campaigns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CampaignId == id);
            if (campaign == null)
            {
                return NotFound();  
            }

            // Get all donations for the campaign
            var donations = _context.Donations
                .Where(d => d.CampaignId == id)
                .Include(d => d.User)
                .ToList();
            
            campaign.Donations = donations;

            return View(campaign);
        }

        // GET: Campaigns/Create
        [AdminAuthorizationFilter]

        public IActionResult Create()
        {
            var users = _context.Users.Where(u => u.Role.RoleName == "Admin");

            ViewData["UserId"] = new SelectList(users, "UserId", "Name");

            return View();
        }

        // POST: Campaigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Create([Bind("CampaignId,Title,Description,StartDate,EndDate,Goal,CurrentAmount,UserId")] Campaign campaign, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file ảnh nếu có
                if (Image != null && Image.Length > 0)
                {
                    // Lấy thời gian hiện tại và định dạng thành ddMMyyyy_HHmmss
                    string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");

                    // Tạo tên tệp bằng cách ghép timestamp và phần mở rộng file
                    var fileName = timestamp + Path.GetExtension(Image.FileName);

                    // Đường dẫn để lưu ảnh trong thư mục "wwwroot/images"
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    // Lưu tệp vào đường dẫn
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào model (chỉ lưu đường dẫn tương đối)
                    campaign.Image = "/images/" + fileName;
                }

                // Thêm dữ liệu vào cơ sở dữ liệu
                _context.Add(campaign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", campaign.UserId);
            return View(campaign);
        }


        // GET: Campaigns/Edit/5
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }

            var users = _context.Users.Where(u => u.Role.RoleName == "Admin");
            ViewData["UserId"] = new SelectList(users, "UserId", "Name", campaign.UserId);
            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Edit(int id, [Bind("CampaignId,Title,Description,StartDate,EndDate,Goal,CurrentAmount,UserId")] Campaign campaign, IFormFile? Image)
        {
            if (id != campaign.CampaignId)
            {
                return NotFound();
             
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image == null)
                    {
                        var existingCampaign = _context.Campaigns.AsNoTracking().FirstOrDefault(c => c.CampaignId == campaign.CampaignId);
                        if (existingCampaign != null)
                        {
                            campaign.Image = existingCampaign.Image;
                        }
                    }

                    // Check if a new image has been uploaded
                    if (Image != null && Image.Length > 0)
                    {
                        // Generate a new file name and save the new image
                        string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
                        var fileName = timestamp + Path.GetExtension(Image.FileName);
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        // Update the campaign's image path
                        campaign.Image = "/images/" + fileName;
                    }
                    // If no new image is uploaded, keep the existing ImagePath
                   
                    // Update the campaign in the context
                    _context.Update(campaign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaignExists(campaign.CampaignId))
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

            var users = _context.Users.Where(u => u.Role.RoleName == "Admin");
            ViewData["UserId"] = new SelectList(users, "UserId", "Name", campaign.UserId);

            return View(campaign);
        }

        // GET: Campaigns/Delete/5
        [AdminAuthorizationFilter]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CampaignId == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        // POST: Campaigns/Delete/5
        [AdminAuthorizationFilter]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign != null)
            {
                _context.Campaigns.Remove(campaign);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampaignExists(int id)
        {
            return _context.Campaigns.Any(e => e.CampaignId == id);
        }
    }
}
