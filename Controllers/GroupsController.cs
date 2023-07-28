using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using static System.Collections.Specialized.BitVector32;

namespace BudgetPortal.Controllers
{
    public class GroupsController : Controller
    {
        private int SelectedSectionNo = 0;
        private readonly ApplicationDbContext _context;
        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index(int Sectionid)
        {   
             Console.Write("Section Number : " + Sectionid);
             SelectedSectionNo= Sectionid;
             var applicationDbContext = _context.BudgetGroups.Where(b =>b.SectionNo == Sectionid).Include(b => b.Sections);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.BudgetGroups == null)
            {
                return NotFound();
            }

            var budgetGroups = await _context.BudgetGroups
                .Include(b => b.Sections)
                .FirstOrDefaultAsync(m => m.GroupNo == id);
            if (budgetGroups == null)
            {
                return NotFound();
            }

            return View(budgetGroups);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo");
            return View();                     
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupNo,GroupName,SectionNo,CreatedDateTime")] BudgetGroups budgetGroups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetGroups);
                await _context.SaveChangesAsync();
                Console.WriteLine("budgetGroups.SectionNo:" + budgetGroups.SectionNo);
                return RedirectToAction(nameof(Index), "Groups",new { sectionid = budgetGroups.SectionNo });
            }
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo", budgetGroups.SectionNo);
            return View(budgetGroups);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.BudgetGroups == null)
            {
                return NotFound();
            }

            var budgetGroups = await _context.BudgetGroups.FindAsync(id);
            if (budgetGroups == null)
            {
                return NotFound();
            }
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo", budgetGroups.SectionNo);
            return View(budgetGroups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GroupNo,GroupName,SectionNo,CreatedDateTime")] BudgetGroups budgetGroups)
        {
            if (id != budgetGroups.GroupNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetGroups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetGroupsExists(budgetGroups.GroupNo))
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
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo", budgetGroups.SectionNo);
            return View(budgetGroups);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.BudgetGroups == null)
            {
                return NotFound();
            }

            var budgetGroups = await _context.BudgetGroups
                .Include(b => b.Sections)
                .FirstOrDefaultAsync(m => m.GroupNo == id);
            if (budgetGroups == null)
            {
                return NotFound();
            }

            return View(budgetGroups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.BudgetGroups == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BudgetGroups'  is null.");
            }
            var budgetGroups = await _context.BudgetGroups.FindAsync(id);
            if (budgetGroups != null)
            {
                _context.BudgetGroups.Remove(budgetGroups);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetGroupsExists(string id)
        {
          return (_context.BudgetGroups?.Any(e => e.GroupNo == id)).GetValueOrDefault();
        }
    }
}
