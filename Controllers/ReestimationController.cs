using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetPortal.Controllers
{
    public class ReestimationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReestimationController> _logger;

        public ReestimationController(ApplicationDbContext context, ILogger<ReestimationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Reestimation()
        {
            //var Year = DateTime.Now.Year;
            var Year = 2023;
            //var Month = DateTime.Now.Month;
            var Month = 10;

            var username = User.Identity.Name;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();
            var LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == DivName)
                                   .Select(x => x.DivisionID).FirstOrDefault();

            var mymodel = new Reestimation();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == (Year - 1)).ToList();

            mymodel.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.DivisionName,
                        Value = x.DivisionID.ToString()

                    }).ToList();
            var IsDivNull = mymodel.DivisionNames.Where(x => x.Selected.Equals("true")).FirstOrDefault();

            mymodel.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            if ((Month > 3 && mymodel.BudgetApprovedStatus != 1) && IsDivNull != null)
            {

                mymodel.IsEnabled = true;
            }
            else
            {
                mymodel.IsEnabled = false;
            }

            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            mymodel.SelectedAcademicYear = AcademicYear;

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
