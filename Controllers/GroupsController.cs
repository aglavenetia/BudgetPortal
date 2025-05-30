﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Authorization;

namespace BudgetPortal.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GroupsController> _logger;
        public GroupsController(ApplicationDbContext context, ILogger<GroupsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Groups
        [Authorize]
        public async Task<IActionResult> Index(int Sectionid, string sortOrder)
        {
            //var applicationDbContext = _context.BudgetGroups.Where(b =>b.SectionNo == Sectionid).Include(b => b.Sections);

            var applicationDbContext = from s in _context.BudgetGroups where(s.SectionNo == Sectionid) select s;

            ViewData["SecNo"] = Sectionid;
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

        // GET: Groups/Details/5
        [Authorize]
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
            ViewData["SectionId"] = _context.BudgetGroups.Where(b => b.GroupNo == id).Select(b => b.SectionNo).FirstOrDefault();
            return View(budgetGroups);
        }


        // GET: Groups/Create
        [Authorize]
        public IActionResult Create(int Sectionid)
        {
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo", Sectionid);
            ViewData["SectionID"] = Sectionid;
            return View();                     
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupNo,GroupName,SectionNo,CreatedDateTime")] BudgetGroups budgetGroups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budgetGroups);
                await _context.SaveChangesAsync();
                //Console.WriteLine("budgetGroups.SectionNo:" + budgetGroups.SectionNo);
                return RedirectToAction(nameof(Index), "Groups",new { sectionid = budgetGroups.SectionNo });
            }
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo", budgetGroups.SectionNo);
            return View(budgetGroups);
        }

        // GET: Groups/Edit/5
        [Authorize]
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
            ViewData["SectionId"] = _context.BudgetGroups.Where(b => b.GroupNo == id).Select(b => b.SectionNo).FirstOrDefault();
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo", budgetGroups.SectionNo);
            return View(budgetGroups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
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
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), "Groups", new { sectionid = budgetGroups.SectionNo });
            }
            ViewData["SectionNo"] = new SelectList(_context.BudgetSections, "SectionNo", "SectionNo", budgetGroups.SectionNo);
            return View(budgetGroups);
        }

        // GET: Groups/Delete/5
        [Authorize]
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
            ViewData["SectionId"] = _context.BudgetGroups.Where(b => b.GroupNo == id).Select(b => b.SectionNo).FirstOrDefault();
            return View(budgetGroups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
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
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index), "Groups", new { sectionid = budgetGroups.SectionNo });
        }

        [Authorize]
        private bool BudgetGroupsExists(string id)
        {
          return (_context.BudgetGroups?.Any(e => e.GroupNo == id)).GetValueOrDefault();
        }
    }
}
