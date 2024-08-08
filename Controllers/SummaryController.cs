using BudgetPortal.Data;
using BudgetPortal.Entities;
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
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();
            var LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == DivName)
                                   .Select(x => x.DivisionID).FirstOrDefault();

            //var Month = DateTime.Now.Month;
            var Month = 10;
            if (Month > 0 && Month < 4)
            {
                //Year = DateTime.Now.Year - 1;
                Year = 2022 - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));
            var mymodel = new MultipleData();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID).Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Divisionss = _context.Division.ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID).Where(x => x.FinancialYear1 == (Year - 1)).ToList();
            mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID).Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
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
            mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;

            return View(mymodel);
        }

        public IActionResult GetDetails(int Year, String Division)
        {
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));

            var LoggedInDivisionID = 0;
            var username = User.Identity.Name;

            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();
            if (username != "admin@test.com")
            {
                LoggedInDivisionID = _context.Division
                                 .Where(d => d.DivisionName == DivName)
                                 .Select(x => x.DivisionID).FirstOrDefault();
            }
            else
            {
                LoggedInDivisionID = _context.Division
                                    .Where(d => d.DivisionName == Division)
                                    .Select(x => x.DivisionID).FirstOrDefault();
            }
            var mymodel = new MultipleData();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID).Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Divisionss = _context.Division.ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID).Where(x => x.FinancialYear1 == (Year - 1)).ToList();
            mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID).Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();

            mymodel.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                        new SelectListItem()
                        {
                            Selected = false,
                            Text = x.DivisionName,
                            Value = x.DivisionID.ToString()

                        }).ToList();

            if (username.Equals("admin@test.com"))
            {
                try
                {
                    mymodel.DivisionNames.Where(x => x.Text.Equals(Division)).Single().Selected = true;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("SelectedDivisionID", "Please select any Division");

                }
            }
            try
            {
                mymodel.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.Year1 + "-" + x.Year2,
                        Value = x.Id.ToString()

                    }).ToList();

                mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("SelectedAcademicYearID", "Please select any Academic Year");
            }
            //mymodel.SelectedAcademicYear = String.Concat(Year.ToString(),"-",(Year+1).ToString());

            return View("Summary", mymodel);

        }
    }
}
