using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectTimeTrackerAuth.Data;
using ProjectTimeTrackerAuth.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProjectTimeTrackerAuth.Controllers
{
    public class ActivityTypesController : Controller
    {
        private readonly TimerContext _context;

        public ActivityTypesController(TimerContext context)
        {
            _context = context;    
        }

        // GET: ActivityTypes
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string query = "SELECT * FROM ActivityType WHERE Username = {0}";
            var activityType = await _context.ActivityTypes
                .FromSql(query, User.Identity.Name)
                .AsNoTracking()
                .ToListAsync();

            if (activityType == null) {
                return NotFound();
            }

            return View(activityType);
        }

        // GET: ActivityTypes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityTypes
                .SingleOrDefaultAsync(m => m.ActivityTypeID == id && m.Username == User.Identity.Name);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // GET: ActivityTypes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ActTypeDescrip,ActTypeProd")] ActivityType activityType)
        {
            activityType.Username = User.Identity.Name;

            if (ModelState.IsValid)
            {
                _context.Add(activityType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(activityType);
        }

        // GET: ActivityTypes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityTypes.SingleOrDefaultAsync(m => m.ActivityTypeID == id && m.Username == User.Identity.Name);
            if (activityType == null)
            {
                return NotFound();
            }
            return View(activityType);
        }

        // POST: ActivityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityTypeID,Username,ActTypeDescrip,ActTypeProd")] ActivityType activityType)
        {
            if (id != activityType.ActivityTypeID || activityType.Username != User.Identity.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTypeExists(activityType.ActivityTypeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(activityType);
        }

        // GET: ActivityTypes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityTypes
                .SingleOrDefaultAsync(m => m.ActivityTypeID == id && m.Username == User.Identity.Name);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // POST: ActivityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityType = await _context.ActivityTypes.SingleOrDefaultAsync(m => m.ActivityTypeID == id && m.Username == User.Identity.Name);
            _context.ActivityTypes.Remove(activityType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ActivityTypeExists(int id)
        {
            return _context.ActivityTypes.Any(e => e.ActivityTypeID == id && e.Username == User.Identity.Name);
        }
    }
}
