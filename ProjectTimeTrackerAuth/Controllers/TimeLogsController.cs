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
    public class TimeLogsController : Controller
    {
        private readonly TimerContext _context;

        public TimeLogsController(TimerContext context)
        {
            _context = context;    
        }

        // GET: TimeLogs
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string query = "SELECT * FROM TimeLog WHERE Username = {0}";
            var timeLog = await _context.TimeLogs
                .FromSql(query, User.Identity.Name)
                .Include(a => a.Activities)
                .AsNoTracking()
                .ToListAsync();

            if (timeLog == null) {
                return NotFound();
            }

            return View(timeLog);
        }

        // GET: TimeLogs/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeLog = await _context.TimeLogs
                .Include(t => t.Activities)
                .SingleOrDefaultAsync(m => m.TimeLogID == id && m.Username == User.Identity.Name);
            if (timeLog == null)
            {
                return NotFound();
            }

            return View(timeLog);
        }

        // GET: TimeLogs/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ActivityID"] = new SelectList(_context.Activities.Where(o => o.Username == User.Identity.Name), "ActivityID", "ActivityName");
            return View();
        }

        // POST: TimeLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityID,StartTime,EndTime")] TimeLog timeLog)
        {
            timeLog.Username = User.Identity.Name;

            if (ModelState.IsValid)
            {
                _context.Add(timeLog);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ActivityID"] = new SelectList(_context.Activities.Where(o => o.Username == User.Identity.Name), "ActivityID", "ActivityName", timeLog.ActivityID);
            return View(timeLog);
        }

        // GET: TimeLogs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeLog = await _context.TimeLogs.SingleOrDefaultAsync(m => m.TimeLogID == id && m.Username == User.Identity.Name);
            if (timeLog == null)
            {
                return NotFound();
            }
            ViewData["ActivityID"] = new SelectList(_context.Activities.Where(o => o.Username == User.Identity.Name), "ActivityID", "ActivityName", timeLog.ActivityID);
            return View(timeLog);
        }

        // POST: TimeLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimeLogID,Username,ActivityID,StartTime,EndTime")] TimeLog timeLog)
        {
            if (id != timeLog.TimeLogID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timeLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeLogExists(timeLog.TimeLogID))
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
            ViewData["ActivityID"] = new SelectList(_context.Activities.Where(o => o.Username == User.Identity.Name), "ActivityID", "Username", timeLog.ActivityID);
            return View(timeLog);
        }

        // GET: TimeLogs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeLog = await _context.TimeLogs
                .Include(t => t.Activities)
                .SingleOrDefaultAsync(m => m.TimeLogID == id && m.Username == User.Identity.Name);
            if (timeLog == null)
            {
                return NotFound();
            }

            return View(timeLog);
        }

        // POST: TimeLogs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timeLog = await _context.TimeLogs.SingleOrDefaultAsync(m => m.TimeLogID == id && m.Username == User.Identity.Name);
            _context.TimeLogs.Remove(timeLog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TimeLogExists(int id)
        {
            return _context.TimeLogs.Any(e => e.TimeLogID == id);
        }
    }
}
