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
    public class LedgersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LedgersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ledgers
        public async Task<IActionResult> Index(String SubGroupid)
        {
            var applicationDbContext = _context.BudgetLedgers.Where (b=> (b.SubGroupNo).Equals(SubGroupid) ).Include(b => b.subGroups);
            ViewData["SubGroupNo"] = SubGroupid;
            var GroupNo = _context.BudgetSubGroups.Where(a=>a.SubGroupNo.Equals(SubGroupid)).Select(a=>a.GroupNo).FirstOrDefault();
            ViewData["GroupNo"] = GroupNo.ToString();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ledgers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.BudgetLedgers == null)
            {
                return NotFound();
            }

            var budgetLedgers = await _context.BudgetLedgers
                .Include(b => b.subGroups)
                .FirstOrDefaultAsync(m => m.LedgerNo == id);
            if (budgetLedgers == null)
            {
                return NotFound();
            }

            return View(budgetLedgers);
        }

        // GET: Ledgers/Create
        public IActionResult Create(String SubGroupid)
        {
            ViewData["SubGroupNo"] = new SelectList(_context.BudgetSubGroups, "SubGroupNo", "SubGroupNo", SubGroupid);
            ViewData["SubGroupID"] = SubGroupid;
            return View();
        }

        // POST: Ledgers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LedgerNo,LedgerName,SubGroupNo,CreatedDateTime")] BudgetLedgers budgetLedgers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetLedgers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Ledgers",new { SubGroupid  = budgetLedgers.SubGroupNo});
            }
            ViewData["SubGroupNo"] = new SelectList(_context.BudgetSubGroups, "SubGroupNo", "SubGroupNo", budgetLedgers.SubGroupNo);
            return View(budgetLedgers);
        }

        // GET: Ledgers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.BudgetLedgers == null)
            {
                return NotFound();
            }

            var budgetLedgers = await _context.BudgetLedgers.FindAsync(id);
            if (budgetLedgers == null)
            {
                return NotFound();
            }
            ViewData["SubGroupNo"] = new SelectList(_context.BudgetSubGroups, "SubGroupNo", "SubGroupNo", budgetLedgers.SubGroupNo);
            return View(budgetLedgers);
        }

        // POST: Ledgers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LedgerNo,LedgerName,SubGroupNo,CreatedDateTime")] BudgetLedgers budgetLedgers)
        {
            if (id != budgetLedgers.LedgerNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetLedgers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetLedgersExists(budgetLedgers.LedgerNo))
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
            ViewData["SubGroupNo"] = new SelectList(_context.BudgetSubGroups, "SubGroupNo", "SubGroupNo", budgetLedgers.SubGroupNo);
            return View(budgetLedgers);
        }

        // GET: Ledgers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.BudgetLedgers == null)
            {
                return NotFound();
            }

            var budgetLedgers = await _context.BudgetLedgers
                .Include(b => b.subGroups)
                .FirstOrDefaultAsync(m => m.LedgerNo == id);
            if (budgetLedgers == null)
            {
                return NotFound();
            }

            return View(budgetLedgers);
        }

        // POST: Ledgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.BudgetLedgers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BudgetLedgers'  is null.");
            }
            var budgetLedgers = await _context.BudgetLedgers.FindAsync(id);
            if (budgetLedgers != null)
            {
                _context.BudgetLedgers.Remove(budgetLedgers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetLedgersExists(string id)
        {
          return (_context.BudgetLedgers?.Any(e => e.LedgerNo == id)).GetValueOrDefault();
        }
    }
}
