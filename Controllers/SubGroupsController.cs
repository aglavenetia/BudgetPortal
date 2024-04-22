using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace BudgetPortal.Controllers
{
    public class SubGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubGroups
        public async Task<IActionResult> Index(String Groupid, String sortOrder)
        {
            //var applicationDbContext = _context.BudgetSubGroups.Where(b => (b.GroupNo).Equals(Groupid)).Include(b => b.groups);

            var applicationDbContext = from b in _context.BudgetSubGroups where((b.GroupNo).Equals(Groupid)) select b;

            ViewData["GroupNo"] = Groupid;
            var SectionNo = _context.BudgetGroups.Where (a => a.GroupNo.Equals(Groupid)).Select (a => a.SectionNo).FirstOrDefault();
            ViewData["SectionNo"] = SectionNo;

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

        // GET: SubGroups/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.BudgetSubGroups == null)
            {
                return NotFound();
            }

            var budgetSubGroups = await _context.BudgetSubGroups
                .Include(b => b.groups)
                .FirstOrDefaultAsync(m => m.SubGroupNo == id);
            if (budgetSubGroups == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = _context.BudgetSubGroups.Where(b => b.SubGroupNo == id).Select(b => b.GroupNo).FirstOrDefault();
            return View(budgetSubGroups);
        }

        // GET: SubGroups/Create
        public IActionResult Create(String Groupid)
        {
            ViewData["GroupNo"] = new SelectList(_context.BudgetGroups, "GroupNo", "GroupNo", Groupid);
            ViewData["GroupID"] = Groupid;
            return View();
        }

        // POST: SubGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubGroupNo,subGroupName,GroupNo,RequireInput,CreatedDateTime")] BudgetSubGroups budgetSubGroups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetSubGroups);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"SubGroups", new { Groupid = budgetSubGroups.GroupNo });
            }
            ViewData["GroupNo"] = new SelectList(_context.BudgetGroups, "GroupNo", "GroupNo", budgetSubGroups.GroupNo);
            
            return View(budgetSubGroups);
        }

        // GET: SubGroups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.BudgetSubGroups == null)
            {
                return NotFound();
            }

            var budgetSubGroups = await _context.BudgetSubGroups.FindAsync(id);
            if (budgetSubGroups == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = _context.BudgetSubGroups.Where(b => b.SubGroupNo == id).Select(b => b.GroupNo).FirstOrDefault();
            ViewData["GroupNo"] = new SelectList(_context.BudgetGroups, "GroupNo", "GroupNo", budgetSubGroups.GroupNo);
            return View(budgetSubGroups);
        }

        // POST: SubGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SubGroupNo,subGroupName,GroupNo,RequireInput,CreatedDateTime")] BudgetSubGroups budgetSubGroups)
        {
            if (id != budgetSubGroups.SubGroupNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetSubGroups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetSubGroupsExists(budgetSubGroups.SubGroupNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), "SubGroups", new { Groupid = budgetSubGroups.GroupNo });
            }
            ViewData["GroupNo"] = new SelectList(_context.BudgetGroups, "GroupNo", "GroupNo", budgetSubGroups.GroupNo);
            return View(budgetSubGroups);
        }

        // GET: SubGroups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.BudgetSubGroups == null)
            {
                return NotFound();
            }

            var budgetSubGroups = await _context.BudgetSubGroups
                .Include(b => b.groups)
                .FirstOrDefaultAsync(m => m.SubGroupNo == id);
            if (budgetSubGroups == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = _context.BudgetSubGroups.Where(b => b.SubGroupNo == id).Select(b => b.GroupNo).FirstOrDefault();

            return View(budgetSubGroups);
        }

        // POST: SubGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.BudgetSubGroups == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BudgetSubGroups'  is null.");
            }
            var budgetSubGroups = await _context.BudgetSubGroups.FindAsync(id);
            if (budgetSubGroups != null)
            {
                _context.BudgetSubGroups.Remove(budgetSubGroups);
            }
            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index), "SubGroups", new { Groupid = budgetSubGroups.GroupNo });
        }

        private bool BudgetSubGroupsExists(string id)
        {
          return (_context.BudgetSubGroups?.Any(e => e.SubGroupNo == id)).GetValueOrDefault();
        }
    }
}
