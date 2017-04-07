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
    public class ActivitiesController : Controller
    {
        private readonly TimerContext _context;

        public ActivitiesController(TimerContext context)
        {
            _context = context;    
        }

        // GET: Activities
        [Authorize]
        public async Task<IActionResult> Index() {

            string query = "SELECT * FROM Activity WHERE Username = {0}";
            var activity = await _context.Activities
                .FromSql(query, User.Identity.Name)
                .Include(a => a.ActivityTypes)
                .AsNoTracking()
                .ToListAsync();

            if (activity == null) {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.ActivityTypes)
                .SingleOrDefaultAsync(m => m.ActivityID == id && m.Username == User.Identity.Name);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes.Where(o => o.Username == User.Identity.Name), "ActivityTypeID", "ActTypeDescrip");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ActivityName,ActivityTypeID")] Activity activity)
        {
            activity.Username = User.Identity.Name;

            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes.Where(o => o.Username == User.Identity.Name), "ActivityTypeID", "ActTypeDescrip", activity.ActivityTypeID);
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.SingleOrDefaultAsync(m => m.ActivityID == id && m.Username == User.Identity.Name);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes.Where(o => o.Username == User.Identity.Name), "ActivityTypeID", "ActTypeDescrip", activity.ActivityTypeID);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityID,Username,ActivityName,ActivityTypeID")] Activity activity)
        {
            if (id != activity.ActivityID || activity.Username != User.Identity.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ActivityID))
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
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes.Where(o => o.Username == User.Identity.Name), "ActivityTypeID", "ActTypeDescrip", activity.ActivityTypeID);
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.ActivityTypes)
                .SingleOrDefaultAsync(m => m.ActivityID == id && m.Username == User.Identity.Name);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.SingleOrDefaultAsync(m => m.ActivityID == id && m.Username == User.Identity.Name);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.ActivityID == id && e.Username == User.Identity.Name);
        }
    }
}
