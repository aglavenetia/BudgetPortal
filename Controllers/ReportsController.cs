using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace BudgetPortal.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Reports()
        {
            var Year = DateTime.Now.Year;
            //var Year = 2022;
            var username = User.Identity.Name;
            //var DivName = _context.Users
            //.Where(x => x.UserName.Equals(username))
            //.Select(x => x.BranchName).First();
            //var LoggedInDivisionID = _context.Division
            //.Where(d => d.DivisionName == DivName)
            //.Select(x => x.DivisionID).FirstOrDefault();
            var Month = DateTime.Now.Month;

            if (Month > 0 && Month < 4)
            {
                Year = DateTime.Now.Year - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var mymodel = new MultipleData();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.FinancialYear1 == Year).ToList();
           
            String[] DivisionTypes = _context.Division.Select(x => x.DivisionType).Distinct().ToArray();

            mymodel.DivisionTypeNames = new List<SelectListItem>();
            for (var i = 1;i<=DivisionTypes.Length;i++)
            {
                mymodel.DivisionTypeNames.Add(
                new SelectListItem
                {
                   Text = DivisionTypes[i-1],
                   Value = i.ToString()
                });
            }
            mymodel.DivisionTypeNames.Add(new SelectListItem
            {
                Text = "All",
                Value = (DivisionTypes.Length + 1).ToString()
            });

            mymodel.DivisionTypeNames.Where(x => x.Text.Equals("Regional Office")).Single().Selected = true;

            mymodel.ReportNames = _context.BudgetReports.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.ReportName,
                        Value = x.ReportID.ToString()

                    }).ToList();
            mymodel.ReportNames.Where(x => x.Text.Equals("Actual")).Single().Selected = true;

            mymodel.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.Year1 + "-" + x.Year2,
                        Value = x.Id.ToString()

                    }).ToList();
            mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;

            return View(mymodel);
        }
    }
}
