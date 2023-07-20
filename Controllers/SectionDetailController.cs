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
    public class SectionDetailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectionDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SectionDetail
        public async Task<IActionResult> Index(int SecNo)
        {


            List<SectionDetails> list = await (from SecList in _context.SectionDetails where SecList.SectionNo == 
                                               SecNo
                                               select new SectionDetails
                                               {
                                                   id = SecList.id,
                                                   SectionNo = SecList.SectionNo,
                                                   SectionName = SecList.SectionName
                                               }).ToListAsync();
              return _context.SectionDetails != null ? 
                          View(list) :
                          Problem("Entity set 'ApplicationDbContext.SectionDetails'  is null.");
        }

        // GET: SectionDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SectionDetails == null)
            {
                return NotFound();
            }

            var sectionDetails = await _context.SectionDetails
                .FirstOrDefaultAsync(m => m.SectionNo == id);
            if (sectionDetails == null)
            {
                return NotFound();
            }

            return View(sectionDetails);
        }

        // GET: SectionDetail/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SectionDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,SectionNo,SectionName")] SectionDetails sectionDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sectionDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sectionDetails);
        }

        // GET: SectionDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SectionDetails == null)
            {
                return NotFound();
            }

            var sectionDetails = await _context.SectionDetails.FindAsync(id);
            if (sectionDetails == null)
            {
                return NotFound();
            }
            return View(sectionDetails);
        }

        // POST: SectionDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,SectionNo,SectionName")] SectionDetails sectionDetails)
        {
            if (id != sectionDetails.SectionNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sectionDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionDetailsExists(sectionDetails.SectionNo))
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
            return View(sectionDetails);
        }

        // GET: SectionDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SectionDetails == null)
            {
                return NotFound();
            }

            var sectionDetails = await _context.SectionDetails
                .FirstOrDefaultAsync(m => m.SectionNo == id);
            if (sectionDetails == null)
            {
                return NotFound();
            }

            return View(sectionDetails);
        }

        // POST: SectionDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SectionDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SectionDetails'  is null.");
            }
            var sectionDetails = await _context.SectionDetails.FindAsync(id);
            if (sectionDetails != null)
            {
                _context.SectionDetails.Remove(sectionDetails);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectionDetailsExists(int id)
        {
          return (_context.SectionDetails?.Any(e => e.SectionNo == id)).GetValueOrDefault();
        }
    }
}
