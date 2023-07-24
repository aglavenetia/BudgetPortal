using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetPortal.Data;
using BudgetPortal.Entities;

namespace BudgetPortal.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sections
        public async Task<IActionResult> Index()
        {
              return _context.Sections != null ? 
                          View(await _context.Sections.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Sections'  is null.");
        }

        // GET: Sections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sections == null)
            {
                return NotFound();
            }

            var sections = await _context.Sections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sections == null)
            {
                return NotFound();
            }

            return View(sections);
        }

        // GET: Sections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SectionNo,SectionName,CreatedDateTime")] Sections sections)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sections);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sections);
        }

        // GET: Sections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sections == null)
            {
                return NotFound();
            }

            var sections = await _context.Sections.FindAsync(id);
            if (sections == null)
            {
                return NotFound();
            }
            return View(sections);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectionNo,SectionName,CreatedDateTime")] Sections sections)
        {
            if (id != sections.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sections);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionsExists(sections.Id))
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
            return View(sections);
        }

        // GET: Sections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sections == null)
            {
                return NotFound();
            }

            var sections = await _context.Sections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sections == null)
            {
                return NotFound();
            }

            return View(sections);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sections == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sections'  is null.");
            }
            var sections = await _context.Sections.FindAsync(id);
            if (sections != null)
            {
                _context.Sections.Remove(sections);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectionsExists(int id)
        {
          return (_context.Sections?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
