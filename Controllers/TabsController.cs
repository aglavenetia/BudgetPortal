using BudgetPortal.Data;
using BudgetPortal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPortal.Models;

namespace BudgetPortal.Controllers
{
    public class TabsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TabsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TabsController

        public ActionResult Index()
       //public async Task<IActionResult> Index()
        {
            var JoinedTable = from BS in _context.BudgetSections
                               join BG in _context.BudgetGroups
                               on BS.SectionNo equals BG.SectionNo into BG1
                               from BG in BG1.DefaultIfEmpty()
                              select new JoinedModel
                               {
                                  section = BS,
                                  group = BG
                               };
            
            //Problem("Entity set 'ApplicationDbContext.BudgetGroups'  is null.");
            //return _context.BudgetGroups != null ?
            //View(await _context.BudgetGroups.ToListAsync()) :
            //Problem("Entity set 'ApplicationDbContext.BudgetGroups'  is null.");
            return View(JoinedTable);
        }

        // GET: TabsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TabsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TabsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TabsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TabsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TabsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TabsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
