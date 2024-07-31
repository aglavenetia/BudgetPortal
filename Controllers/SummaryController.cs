using BudgetPortal.Data;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetPortal.Controllers
{
    public class SummaryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SummaryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Summary()
        {
            //var Year = DateTime.Now.Year;
            var Year = 2022;
            var username = User.Identity.Name;

            //var Month = DateTime.Now.Month;
            var Month = 10;
            if (Month > 0 && Month < 4)
            {
                //Year = DateTime.Now.Year - 1;
                Year = 2022 - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));
            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Divisionss = _context.Division.ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.FinancialYear1 == (Year - 1)).ToList();
            String[] DivisionTypes = _context.Division.Select(x => x.DivisionType).Distinct().ToArray();

            mymodel.DivisionTypeNames = new List<SelectListItem>();
            for (var i = 1; i <= DivisionTypes.Length; i++)
            {
                mymodel.DivisionTypeNames.Add(
                new SelectListItem
                {
                    Text = DivisionTypes[i - 1],
                    Value = i.ToString()
                });
            }
            //mymodel.DivisionTypeNames.Add(new SelectListItem
            //{
            //    Text = "All",
            //    Value = (DivisionTypes.Length + 1).ToString()
            //});

            mymodel.DivisionTypeNames.Where(x => x.Text.Equals("Regional Office")).Single().Selected = true;
            //mymodel.SelectedDivisionTypeName = mymodel.DivisionTypeNames.Where(x => x.Selected == true).ToString();

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
