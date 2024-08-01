using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieLibrary.Models;

namespace MovieLibrary.Controllers
{
    public class AvailabilitiesController : Controller
    {
        private readonly moviesLibraryDB _context;

        public AvailabilitiesController(moviesLibraryDB context)
        {
            _context = context;
        }

        // GET: Availabilities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Availabilities.ToListAsync());
        }

        // GET: Availabilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var availability = await _context.Availabilities
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.AvailabilityId == id);

            if (availability == null)
            {
                return NotFound();
            }

            return View(availability);
        }

        // GET: Availabilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Availabilities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvailabilityId,AvailabilityName")] Availabilities availabilities)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Έλεγχος για το αν το πεδίο AvailabilityName δεν είναι κενό ή null
                    if (string.IsNullOrWhiteSpace(availabilities.AvailabilityName))
                    {
                        ModelState.AddModelError("AvailabilityName", "Please give a name");
                        return View(availabilities);
                    }

                    _context.Add(availabilities);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(availabilities);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occurred while saving your data. Please try again.");
                return View(availabilities);
            }
        }

        // GET: Availabilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var availability = await _context.Availabilities
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(m => m.AvailabilityId == id);
            if (availability == null)
            {
                return NotFound();
            }

            return View(availability);
        }

        // POST: Availabilities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AvailabilityId,AvailabilityName")] Availabilities availability)
        {
            if (id != availability.AvailabilityId)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(availability.AvailabilityName))
            {
                var existingAvailability = await _context.Availabilities.AsNoTracking().FirstOrDefaultAsync(a => a.AvailabilityId == id);
                if (existingAvailability != null)
                {
                    availability.AvailabilityName = existingAvailability.AvailabilityName;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(availability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvailabilitiesExists(availability.AvailabilityId))
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
            return View(availability);
        }


        // GET: Availabilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var availabilities = await _context.Availabilities
                .FirstOrDefaultAsync(m => m.AvailabilityId == id);
            if (availabilities == null)
            {
                return NotFound();
            }

            return View(availabilities);
        }

        // POST: Availabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var availability = await _context.Availabilities
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(a => a.AvailabilityId == id);

            if (availability == null)
            {
                return NotFound();
            }

            if (availability.Movie != null && availability.Movie.Any())
            {
                TempData["ErrorMessage"] = "You cannot delete this Platform because there are movies that belong to this Platform.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            _context.Availabilities.Remove(availability);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AvailabilitiesExists(int id)
        {
            return _context.Availabilities.Any(e => e.AvailabilityId == id);
        }
    }
}
