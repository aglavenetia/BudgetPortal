using BudgetPortal.Data;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetPortal.Controllers
{
    public class InterimRevisionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InterimRevisionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult InterimRev()
        {
            var Year = DateTime.Now.Year;

            var username = User.Identity.Name;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).First();
            var LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == DivName)
                                   .Select(x => x.DivisionID).FirstOrDefault();
            var Month = DateTime.Now.Month;

            if (Month > 0 && Month < 4)
            {
                Year = DateTime.Now.Year - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var mymodel = new MultipleData();
            mymodel.SelectedAcademicYear = AcademicYear;
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Filess = _context.BudgetFiles.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == (Year - 1)).ToList();
            mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();

            int FinalApproved = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
            int SubmittedForApproval = (from a in mymodel.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
            int NumberOfGroups = (from a in mymodel.Groupss select a.GroupNo).Count();
            int PendingForFinalSubmission = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();

            if (FinalApproved > 0)
            {
                mymodel.ApprovedMessage = "Budget Details Approved for the Financial Year " + AcademicYear + "!!!";
                mymodel.WaitingForApprovalMessage = " ";
            }
            else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && SubmittedForApproval != 0)
            {
                mymodel.WaitingForApprovalMessage = "Budget Details for the Financial Year " + AcademicYear + " is pending with AC&BW for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForFinalSubmission > 0)
            {
                mymodel.WaitingForApprovalMessage = "Budget Details for the Financial Year " + AcademicYear + " is pending with CMD for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else
            {
                mymodel.ApprovedMessage = " ";
                mymodel.WaitingForApprovalMessage = " ";
            }


            mymodel.PreviousYearAdminCount = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year - 1).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();



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
    }
}
