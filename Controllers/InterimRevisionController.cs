using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
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
            var Month = DateTime.Now.Month;

            var username = User.Identity.Name;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();
            var LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == DivName)
                                   .Select(x => x.DivisionID).FirstOrDefault();
            
            var mymodel = new InterimRevision();
            
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
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

            if (Month >3 && Month < 10)
            {
                Year = DateTime.Now.Year - 1;
                mymodel.IsEnabled= true;
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


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult InterimRev(InterimRevision IM)
        {
            Boolean valid = true;
            //Update Budget Details for Admin User
            if (User.Identity.Name.Equals("admin@test.com"))
            {
                int index = IM.SubGroupNameOrLedgerName.IndexOf(IM.SubGroupLedgerName);
                var username = User.Identity.Name;
                var DivName = IM.SelectedDivisionName.ToString();
                var SelectedDivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault();
                var splitAcademicYear = IM.SelectedAcademicYear.ToString().Split("-");
                var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(IM.SectionName))
                                  .Select(x => x.SectionNo).FirstOrDefault();
                var GroupNumber = _context.BudgetGroups
                         .Where(x => x.GroupName.Equals(IM.GroupName))
                         .Select(x => x.GroupNo).FirstOrDefault();

                var SubGroupNumber = _context.BudgetSubGroups
                             .Where(x => x.SubGroupNo.Equals(IM.SubGroupNameOrLedgerName[index]))
                             .Select(x => x.SubGroupNo).FirstOrDefault();
                var LedgerNumber = 0.ToString();

                if (SubGroupNumber is null)
                {
                     LedgerNumber = _context.BudgetLedgers
                                       .Where(x => x.LedgerNo.Equals(IM.SubGroupLedgerName))
                                       .Select(x => x.LedgerNo).FirstOrDefault();
                    SubGroupNumber = _context.BudgetLedgers
                                       .Where(x => x.LedgerNo.Equals(LedgerNumber))
                                       .Select(x => x.SubGroupNo).FirstOrDefault();
                }
                else
                { 
                     LedgerNumber = 0.ToString();
                }

                ModelState.Remove("SubGroupLedgerName");

                if (ModelState.IsValid)
                {
                    var result = new BudgetDetails();

                    result.DivisionID = SelectedDivisionID;
                    result.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                    result.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                    result.SectionNumber = Convert.ToInt32(SectionNumber);
                    result.GroupNumber = GroupNumber;
                    result.SubGroupNumber = SubGroupNumber;
                    result.LedgerNumber = LedgerNumber;
                    result.InterimRevEst = IM.InterimRev[index];
                    result.ProvisionalRevEst = Convert.ToDecimal(IM.BudEstCurrFin[index] + IM.InterimRev[index]);

                    _context.BudgetDetails.Add(result);
                    _context.SaveChanges();
                }

                var DivisionID = _context.Division
                                             .Where(d => d.DivisionName == DivName)
                                             .Select(x => x.DivisionID).FirstOrDefault();
                IM.Sectionss = _context.BudgetSections.ToList();
                IM.Groupss = _context.BudgetGroups.ToList();
                IM.SubGroupss = _context.BudgetSubGroups.ToList();
                IM.Ledgerss = _context.BudgetLedgers.ToList();
                IM.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == DivisionID)
                                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
                IM.ProvisionalRE[index] = Convert.ToDecimal(IM.BudEstCurrFin[index] + IM.InterimRev[index]);


                /*int FinalApproved = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
                int SubmittedForApproval = (from a in MD.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
                int NumberOfGroups = (from a in MD.Groupss select a.GroupNo).Count();
                int PendingForFinalSubmission = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();

                if (FinalApproved > 0)
                {
                    MD.ApprovedMessage = "Budget Details Approved for the Financial Year " + MD.SelectedAcademicYear + "!!!";
                    MD.WaitingForApprovalMessage = " ";
                }
                else if (FinalApproved == 0 && SubmittedForApproval <= NumberOfGroups && SubmittedForApproval != 0)
                {
                    MD.WaitingForApprovalMessage = "Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForFinalSubmission > 0)
                {
                    MD.WaitingForApprovalMessage = "Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with CMD for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else
                {
                    MD.ApprovedMessage = " ";
                    MD.WaitingForApprovalMessage = " ";
                }


                MD.PreviousYearAdminCount = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                     .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();*/

                var Month = DateTime.Now.Month;
                if (Month > 3 && Month < 10)
                {
                    IM.IsEnabled = true;
                }
                IM.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == DivisionID)
                                      .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
                IM.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                        new SelectListItem()
                        {
                            Selected = false,
                            Text = x.DivisionName,
                            Value = x.DivisionID.ToString()

                        }).ToList();

                IM.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.Year1 + "-" + x.Year2,
                        Value = x.Id.ToString()

                    }).ToList();
                IM.AcademicYears.Where(x => x.Text.Equals(IM.SelectedAcademicYear.ToString())).Single().Selected = true;

                IM.DivisionNames.Where(x => x.Text.Equals(IM.SelectedDivisionName.ToString())).Single().Selected = true;

                return View("InterimRev", IM);

            }

            return View("InterimRev", IM);

        }

        [HttpGet]
        public IActionResult GetDetails(int Year, String Division)
        {

            var username = User.Identity.Name;
            var LoggedInDivisionID = 0;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();

           
                LoggedInDivisionID = _context.Division
                                    .Where(d => d.DivisionName == Division)
                                    .Select(x => x.DivisionID).FirstOrDefault();
            
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var mymodel = new InterimRevision();
            mymodel.SelectedAcademicYear = AcademicYear;
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                         .Where(x => x.FinancialYear1 == (Year - 1)).ToList();

            var Month = DateTime.Now.Month;
            if (Month > 3 && Month < 10)
            {
                mymodel.IsEnabled = true;
            }
            /*int FinalApproved = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
            int SubmittedForApproval = (from a in mymodel.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
            int NumberOfGroups = (from a in mymodel.Groupss select a.GroupNo).Count();
            int PendingForFinalSubmission = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();

            if (FinalApproved > 0)
            {
                mymodel.ApprovedMessage = "* Budget Details Approved for the Financial Year " + AcademicYear + "!!!";
                mymodel.WaitingForApprovalMessage = " ";
            }
            else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && SubmittedForApproval != 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + AcademicYear + " is pending with AC&BW for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForFinalSubmission > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + AcademicYear + " is pending with CMD for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else
            {
                mymodel.ApprovedMessage = " ";
                mymodel.WaitingForApprovalMessage = " ";
            }

            mymodel.PreviousYearAdminCount = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                     .Where(x => x.FinancialYear1 == Convert.ToInt32(Year - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();*/

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
            //ViewBag.SelectedAcademicYearID = mymodel.AcademicYears;
            try
            {
                mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("SelectedAcademicYearID", "Please select any Academic Year");

            }
            //mymodel.SelectedAcademicYearID = String.Concat(Year.ToString(),"-",(Year+1).ToString());
            
            try
            {
                    mymodel.DivisionNames.Where(x => x.Text.Equals(Division)).Single().Selected = true;
            }
            catch (Exception ex)
            {
                    ModelState.AddModelError("SelectedDivisionID", "Please select any Division");

            }
            

            return View("InterimRev", mymodel);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(InterimRevision IM)
        {
            var username = User.Identity.Name;
            var DivName = " ";
            var DivisionID = " ";
            int index = IM.SubGroupNameOrLedgerName.IndexOf(IM.SubGroupLedgerName);
            var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(IM.SectionName))
                                  .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = _context.BudgetGroups
                     .Where(x => x.GroupName.Equals(IM.GroupName))
                     .Select(x => x.GroupNo).FirstOrDefault();

            var SubGroupNumber = _context.BudgetSubGroups
                     .Where(x => x.SubGroupNo.Equals(IM.SubGroupLedgerName))
                     .Select(x => x.SubGroupNo).FirstOrDefault();

            
            var LedgerNumber = _context.BudgetLedgers
                     .Where(x => x.LedgerNo.Equals(IM.SubGroupLedgerName))
                     .Select(x => x.LedgerNo).FirstOrDefault();

            if (LedgerNumber != null)
            {
                SubGroupNumber = _context.BudgetLedgers
                    .Where(x => x.LedgerNo.Equals(IM.SubGroupLedgerName))
                    .Select(x => x.SubGroupNo).FirstOrDefault();
            }
            else
            {
                 LedgerNumber = 0.ToString();
            }

            if (User.Identity.Name.Equals("admin@test.com"))
            {
                DivName = IM.SelectedDivisionName.ToString();
                DivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }

            var splitAcademicYear = IM.SelectedAcademicYear.ToString().Split("-");
            ModelState.Remove("ACAndBWPropRECurrFin");
            ModelState.Remove("ACAndBWPropRENxtFin");
            var result = new BudgetDetails();
            result = _context.BudgetDetails
                                      .Where(b => (b.DivisionID == Convert.ToInt32(DivisionID))
                                               && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                               && (b.SectionNumber == SectionNumber)
                                               && (b.GroupNumber == GroupNumber)
                                               && (b.SubGroupNumber == SubGroupNumber)
                                               && (b.LedgerNumber == LedgerNumber)).FirstOrDefault();
            _context.BudgetDetails.Remove(result);
            _context.SaveChanges();

            var Month = DateTime.Now.Month;
            if (Month > 3 && Month < 10)
            {
                IM.IsEnabled = true;
            }

            IM.Sectionss = _context.BudgetSections.ToList();
            IM.Groupss = _context.BudgetGroups.ToList();
            IM.SubGroupss = _context.BudgetSubGroups.ToList();
            IM.Ledgerss = _context.BudgetLedgers.ToList();
            IM.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            IM.InterimRev[index] = Convert.ToDecimal(0.00);
            IM.ProvisionalRE[index] = Convert.ToDecimal(0.00);

            IM.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
            IM.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.DivisionName,
                        Value = x.DivisionID.ToString()

                    }).ToList();

            IM.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                new SelectListItem()
                {
                    Selected = false,
                    Text = x.Year1 + "-" + x.Year2,
                    Value = x.Id.ToString()

                }).ToList();
            IM.AcademicYears.Where(x => x.Text.Equals(IM.SelectedAcademicYear.ToString())).Single().Selected = true;

            return View("InterimRev", IM);
        }
    }
}
