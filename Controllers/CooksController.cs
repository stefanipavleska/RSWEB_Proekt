using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProekt.Data;
using MVCProekt.Models;
using MVCProekt.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MVCProekt.Controllers
{
    public class CooksController : Controller
    {
        private readonly MVCProektContext _context;

        public CooksController(MVCProektContext context)
        {
            _context = context;
        }

        // GET: Cooks
        public async Task<IActionResult> Index(string cookGender, string searchStringName, string searchStringSurname)
        {
            IQueryable<Cook> cooks = _context.Cook.AsQueryable();
            IQueryable<string> genderQuery = _context.Cook.OrderBy(m => m.Gender).Select(m => m.Gender).Distinct();
            if (!string.IsNullOrEmpty(searchStringName))
            {
                cooks = cooks.Where(s => s.FirstName.Contains(searchStringName));
            }
            if (!string.IsNullOrEmpty(searchStringSurname))
            {
                cooks = cooks.Where(s => s.LastName.Contains(searchStringSurname));
            }
            if (!string.IsNullOrEmpty(cookGender))
            {
                cooks = cooks.Where(x => x.Gender == cookGender);
            }
            var cookGenderNameSurnameVM = new CookNameSurnameGender
            {
                Gender = new SelectList(await genderQuery.ToListAsync()),
                Cooks = await cooks.ToListAsync()
            };
            return View(cookGenderNameSurnameVM);
        }

        // GET: Cooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cook == null)
            {
                return NotFound();
            }

            var cook = await _context.Cook
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cook == null)
            {
                return NotFound();
            }

            return View(cook);
        }

        // GET: Cooks/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Gender")] Cook cook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cook);
        }

        // GET: Cooks/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cook == null)
            {
                return NotFound();
            }

            var cook = await _context.Cook.FindAsync(id);
            if (cook == null)
            {
                return NotFound();
            }
            return View(cook);
        }

        // POST: Cooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Gender")] Cook cook)
        {
            if (id != cook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CookExists(cook.Id))
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
            return View(cook);
        }

        // GET: Cooks/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cook == null)
            {
                return NotFound();
            }

            var cook = await _context.Cook
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cook == null)
            {
                return NotFound();
            }

            return View(cook);
        }

        // POST: Cooks/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cook == null)
            {
                return Problem("Entity set 'MVCProektContext.Cook'  is null.");
            }
            var cook = await _context.Cook.FindAsync(id);
            if (cook != null)
            {
                _context.Cook.Remove(cook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CookExists(int id)
        {
          return (_context.Cook?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
