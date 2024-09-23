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
    public class SubLedgersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SubLedgersController> _logger;

        public SubLedgersController(ApplicationDbContext context, ILogger<SubLedgersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Ledgers
        [Authorize]
        public async Task<IActionResult> Index(String SubGroupid, String sortOrder)
        {
            //var applicationDbContext = _context.BudgetLedgers.Where (b=> (b.SubGroupNo).Equals(SubGroupid) ).Include(b => b.subGroups);

            var applicationDbContext = from b in _context.BudgetLedgers where(b.SubGroupNo.Equals(SubGroupid)) select b;
            ViewData["SubGroupNo"] = SubGroupid;
            var GroupNo = _context.BudgetSubGroups.Where(a=>a.SubGroupNo.Equals(SubGroupid)).Select(a=>a.GroupNo).FirstOrDefault();
            ViewData["GroupNo"] = GroupNo.ToString();

            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "Date":
                    applicationDbContext = applicationDbContext.OrderBy(s => s.CreatedDateTime);
                    break;
                case "date_desc":
                    applicationDbContext = applicationDbContext.OrderByDescending(s => s.CreatedDateTime);
                    break;
                default:
                    applicationDbContext = applicationDbContext.OrderBy(s => s.CreatedDateTime);
                    break;
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ledgers/Details/5
        [Authorize]
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
            ViewData["SubGroupId"] = _context.BudgetLedgers.Where(b => b.LedgerNo == id).Select(b => b.SubGroupNo).FirstOrDefault();
            return View(budgetLedgers);
        }

        // GET: Ledgers/Create
        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LedgerNo,LedgerName,SubGroupNo,CreatedDateTime")] BudgetSubLedgers budgetLedgers)
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
        [Authorize]
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
            ViewData["SubGroupId"] = _context.BudgetLedgers.Where(b => b.LedgerNo == id).Select(b => b.SubGroupNo).FirstOrDefault();
            ViewData["SubGroupNo"] = new SelectList(_context.BudgetSubGroups, "SubGroupNo", "SubGroupNo", budgetLedgers.SubGroupNo);
            return View(budgetLedgers);
        }

        // POST: Ledgers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LedgerNo,LedgerName,SubGroupNo,CreatedDateTime")] BudgetSubLedgers budgetLedgers)
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
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), "Ledgers", new { SubGroupid = budgetLedgers.SubGroupNo });
            }
            ViewData["SubGroupNo"] = new SelectList(_context.BudgetSubGroups, "SubGroupNo", "SubGroupNo", budgetLedgers.SubGroupNo);
            return View(budgetLedgers);
        }

        // GET: Ledgers/Delete/5
        [Authorize]
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
            ViewData["SubGroupId"] = _context.BudgetLedgers.Where(b => b.LedgerNo == id).Select(b => b.SubGroupNo).FirstOrDefault();
            return View(budgetLedgers);
        }

        // POST: Ledgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
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
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index), "Ledgers", new { SubGroupid = budgetLedgers.SubGroupNo });
        }

        [Authorize]
        private bool BudgetLedgersExists(string id)
        {
          return (_context.BudgetLedgers?.Any(e => e.LedgerNo == id)).GetValueOrDefault();
        }
    }
}
