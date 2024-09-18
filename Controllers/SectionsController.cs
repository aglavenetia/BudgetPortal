using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BudgetPortal.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SectionsController> _logger;

        public SectionsController(ApplicationDbContext context, ILogger<SectionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sections
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.BudgetSections != null ? 
                          View(await _context.BudgetSections.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.BudgetSections'  is null.");
        }

        // GET: Sections/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BudgetSections == null)
            {
                return NotFound();
            }

            var budgetSections = await _context.BudgetSections
                .FirstOrDefaultAsync(m => m.SectionNo == id);
            if (budgetSections == null)
            {
                return NotFound();
            }

            return View(budgetSections);
        }

        // GET: Sections/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SectionNo,SectionName,CreatedDateTime")] BudgetSections budgetSections)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetSections);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budgetSections);
        }

        // GET: Sections/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BudgetSections == null)
            {
                return NotFound();
            }

            var budgetSections = await _context.BudgetSections.FindAsync(id);
            if (budgetSections == null)
            {
                return NotFound();
            }
            return View(budgetSections);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SectionNo,SectionName,CreatedDateTime")] BudgetSections budgetSections)
        {
            if (id != budgetSections.SectionNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetSections);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetSectionsExists(budgetSections.SectionNo))
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
            return View(budgetSections);
        }

        // GET: Sections/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BudgetSections == null)
            {
                return NotFound();
            }

            var budgetSections = await _context.BudgetSections
                .FirstOrDefaultAsync(m => m.SectionNo == id);
            if (budgetSections == null)
            {
                return NotFound();
            }

            return View(budgetSections);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BudgetSections == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BudgetSections'  is null.");
            }
            var budgetSections = await _context.BudgetSections.FindAsync(id);
            if (budgetSections != null)
            {
                _context.BudgetSections.Remove(budgetSections);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool BudgetSectionsExists(int id)
        {
          return (_context.BudgetSections?.Any(e => e.SectionNo == id)).GetValueOrDefault();
        }
    }
}
