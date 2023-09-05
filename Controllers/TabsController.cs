using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BudgetPortal.Controllers
{
    public class TabsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TabsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var mymodel = new MultipleData();
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.ToList();
            mymodel.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.DivisionName,
                        Value = x.DivisionID.ToString()

                    }).ToList();
            mymodel.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.Year1 + "-" + x.Year2,
                        Value = x.Id.ToString()

                    }).ToList();


            return View(mymodel);
        }

        [HttpPost]
        public IActionResult Index(MultipleData MD)
        {

            var username = User.Identity.Name;
            /*var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
            var DivName = _context.Users.Where(x => x.UserName.Equals(username))
                           .Select(x => x.BranchName);
            var SelectedDivisionID = _context.Division.Where(x => x.DivisionName.Equals(DivName))
                                    .Select(x => x.DivisionID);

            for (int i = 0; i < MD.BudEstCurrFin.Count(); i++)
            {
                var dataModel = _context.BudgetDetails;

                dataModel.DivisionID = SelectedDivisionID;
                dataModel.FinancialYear1 = splitAcademicYear[0];
                dataModel.FinancialYear2 = splitAcademicYear[1];
                dataModel.BudEstCurrFin = MD.BudEstCurrFin[i];
                dataModel.ActPrevFin = MD.ActPrevFin[i];
                dataModel.ActCurrFinTill2ndQuart = MD.ActCurrFinTill2ndQuart[i];
                dataModel.RevEstCurrFin = MD.RevEstCurrFin[i];
                dataModel.BudgEstNexFin = MD.BudgEstNexFin[i];
                dataModel.Justification = MD.Justification[i];
                dataModel.SectionNumber = MD.SectionNumber;
                dataModel.GroupNumber = MD.GroupNumber;

                dataModel.SaveChanges();
            }
            /*var username = User.Identity.Name;
             var pubNameQuery = from n in _context.Users.AsNoTracking()
                                where n.UserName == username
                                select n.BranchName;
             string DivisionName = await pubNameQuery.SingleAsync();
             String DivisionID = from n in _context.Division
                                 where n.DivisionName == DivisionName
                                 select n.DivisionID;
             String[] splitAcademicYear = MD.SelectedAcademicYear.Split("-");

                 BudgetDetails dataModel = _context.BudgetDetails.Where(x => x.Id == MD.Id).First();

                 dataModel.DivisionID = DivisionID;
                 dataModel.FinancialYear1 = splitAcademicYear[0];
                 dataModel.FinancialYear2 = splitAcademicYear[1];
                 _context.SaveChanges();
            return View(MD);
          }*/




        }
    }
}
