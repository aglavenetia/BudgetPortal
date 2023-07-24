using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using WebGrease.Css.Ast.Selectors;

namespace BudgetPortal.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index(int SectionNumber)
        {
            List<Groups> list = await (from Group in _context.Groups
                                         where Group.sections.SectionNo == SectionNumber
                                       select new Groups
                                         {
                                           Id = Group.Id,
                                           GroupNo = Group.GroupNo,
                                           GroupName = Group.GroupName,
                                           CreatedDateTime = Group.CreatedDateTime
                                           //,sections.SectionNo = SectionNumber
                                       }).ToListAsync();
              return _context.Groups != null ? 
                          View(list) :
                          Problem("Entity set 'ApplicationDbContext.Groups'  is null.");
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GroupNo,GroupName,CreatedDateTime")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groups);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groups);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }
            return View(groups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupNo,GroupName,CreatedDateTime")] Groups groups)
        {
            if (id != groups.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsExists(groups.Id))
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
            return View(groups);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Groups'  is null.");
            }
            var groups = await _context.Groups.FindAsync(id);
            if (groups != null)
            {
                _context.Groups.Remove(groups);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupsExists(int id)
        {
          return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
