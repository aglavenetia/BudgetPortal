using BudgetPortal.Data;
using BudgetPortal.Entities; 
using BudgetPortal.ViewModel;
using Microsoft.Ajax.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
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
            mymodel.Divisionss = _context.Division.ToList();
            return View(mymodel);
         
        }


        
    }
}
