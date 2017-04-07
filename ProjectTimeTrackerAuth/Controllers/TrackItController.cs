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
    public class TrackItController : Controller
    {
        private readonly TimerContext _context;

        public TrackItController(TimerContext context)
        {
            _context = context;    
        }

        // GET: TrackIt
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //get last time log with no end time
            string query = "SELECT TOP 1 * FROM TimeLog WHERE Username = {0} and EndTime is null order by StartTime desc";
            var timeLog = await _context.TimeLogs
                .FromSql(query, User.Identity.Name)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            ViewData["LastTimeLog"] = timeLog;

            //get list of activities
            ViewData["ActivityID"] = new SelectList(_context.Activities.Where(o => o.Username == User.Identity.Name), "ActivityID", "ActivityName");

            return View();
        }

        // POST: TrackIt/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityID")] TimeLog timeLog)
        {
            if (ModelState.IsValid)
            {
                //get last time log with no end time
                string query = "SELECT TOP 1 * FROM TimeLog WHERE Username = {0} and EndTime is null order by StartTime desc";
                try {
                    var endTimeLog = await _context.TimeLogs
                        .FromSql(query, User.Identity.Name)
                        .AsNoTracking()
                        .SingleOrDefaultAsync();
                    if (endTimeLog != null) {
                        endTimeLog.EndTime = DateTime.Now;
                        _context.Update(endTimeLog);
                        await _context.SaveChangesAsync();
                    }
                } catch (DbUpdateConcurrencyException) {
                    if (!TimeLogExists(timeLog.TimeLogID)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }


                timeLog.Username = User.Identity.Name;
                timeLog.StartTime = DateTime.Now;
                _context.Add(timeLog);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ActivityID"] = new SelectList(_context.Activities, "ActivityID", "ActivityID", timeLog.ActivityID);
            return View(timeLog);
        }

        // POST: TrackIt/StopActivity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TimeLogID,Username,StartTime,ActivityID")] TimeLog timeLog) {
            if (ModelState.IsValid) {
                try {
                    timeLog.Username = User.Identity.Name;
                    timeLog.EndTime = DateTime.Now;
                    _context.Update(timeLog);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!TimeLogExists(timeLog.TimeLogID)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        private bool TimeLogExists(int id)
        {
            return _context.TimeLogs.Any(e => e.TimeLogID == id);
        }
    }
}
