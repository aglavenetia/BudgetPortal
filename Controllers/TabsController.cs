using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace BudgetPortal.Controllers
{
    public class TabsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TabsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {

            var mymodel = new MultipleData();
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            return View(mymodel);

            /* var JoinedTable = new List<JoinedModel>();
              JoinedTable = (from BS in _context.BudgetSections.ToList()
                                join BG in _context.BudgetGroups.ToList()
                                on BS.SectionNo equals BG.SectionNo
                                select new JoinedModel
                                {
                                    SectionNo = BS.SectionNo,
                                    SectionName = BS.SectionName,
                                    GroupNo = BG.GroupNo,
                                    GroupName = BG.GroupName
                                }).ToList();

             //Problem("Entity set 'ApplicationDbContext.BudgetGroups'  is null.");
             //return _context.BudgetGroups != null ?
             //View(await _context.BudgetGroups.ToListAsync()) :
             //Problem("Entity set 'ApplicationDbContext.BudgetGroups'  is null.");
             return View(JoinedTable);
             //return View();*/
        }
        
    }
}
