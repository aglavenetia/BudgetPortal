﻿using BudgetPortal.Data;
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
        private readonly ILogger<InterimRevisionController> _logger;

        public InterimRevisionController(ApplicationDbContext context, ILogger<InterimRevisionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult InterimRev()
        {
            //var Year = DateTime.Now.Year;
            var Year = GlobalVariables.Year;
            //var Month = DateTime.Now.Month;
            var Month = GlobalVariables.Month;
            if (Month > 0 && Month < 4)
            {
                /*Year = DateTime.Now.Year - 1;*/
                Year = Year - 1;
            }


            var username = User.Identity.Name;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();
            var LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == DivName)
                                   .Select(x => x.DivisionID).FirstOrDefault();
            
            var mymodel = new InterimRevision();
            
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

            if ((Month >3 && mymodel.BudgetApprovedStatus != 1) && IsDivNull!=null)
            {
                
                mymodel.IsEnabled= true;
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


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult InterimRev(InterimRevision IM)
        {
            //Boolean valid = true;
            
            //Saves Interim Revision Details
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
                ModelState.Remove("EditEnabled");

                if (ModelState.IsValid)
                {
                    if (IM.EditEnabled != IM.SubGroupLedgerName)
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
                    else
                    {
                        var result = new BudgetDetails();
                        result = _context.BudgetDetails
                                                  .Where(b => (b.DivisionID == Convert.ToInt32(SelectedDivisionID))
                                                           && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                                           && (b.SectionNumber == SectionNumber)
                                                           && (b.GroupNumber == GroupNumber)
                                                           && (b.SubGroupNumber == SubGroupNumber)
                                                           && (b.LedgerNumber == LedgerNumber)).FirstOrDefault();
                        result.InterimRevEst = IM.InterimRev[index];
                        result.ProvisionalRevEst = Convert.ToDecimal(IM.BudEstCurrFin[index] + IM.InterimRev[index]);

                        _context.BudgetDetails.Update(result);
                        _context.SaveChanges();
                    }

                }

                var DivisionID = _context.Division
                                             .Where(d => d.DivisionName == DivName)
                                             .Select(x => x.DivisionID).FirstOrDefault();
                IM.EditEnabled = null;
                IM.Sectionss = _context.BudgetSections.ToList();
                IM.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
                IM.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
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

                //var Month = DateTime.Now.Month;
                var Month = GlobalVariables.Month;

                IM.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();
                
                if (Month > 0 && Month < 4)
                {
                    /*Year = DateTime.Now.Year - 1;*/
                    splitAcademicYear[0] = (Convert.ToInt32(splitAcademicYear[0]) + 1).ToString();
                }
                if ((Month > 3 && IM.BudgetApprovedStatus!=1) && (Convert.ToInt32(splitAcademicYear[0]) == GlobalVariables.Year))
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
                ModelState.Clear();
                return View("InterimRev", IM);

            }

            return View("InterimRev", IM);

        }

        [HttpGet]
        [Authorize]
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
            mymodel.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                         .Where(x => x.FinancialYear1 == (Year - 1)).ToList();

            //var Month = DateTime.Now.Month;
            
            
            mymodel.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();
            
            var Month = GlobalVariables.Month;
            if (Month > 0 && Month < 4)
            {
                /*Year = DateTime.Now.Year - 1;*/
                Year = Year + 1;
            }
            //if ((Month > 3 && Month < 1) && (Year == DateTime.Now.Year))
            if ((Month > 3 && mymodel.BudgetApprovedStatus!=1) && (Year == GlobalVariables.Year))
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

            //var Month = DateTime.Now.Month;
            var Month = GlobalVariables.Month;

            

            IM.Sectionss = _context.BudgetSections.ToList();
            IM.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            IM.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
            IM.Ledgerss = _context.BudgetLedgers.ToList();
            IM.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            IM.InterimRev[index] = Convert.ToDecimal(0.00);
            IM.ProvisionalRE[index] = Convert.ToDecimal(0.00);

            IM.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();

            IM.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            if (Month > 0 && Month < 4)
            {
                
                splitAcademicYear[0] = (Convert.ToInt32(splitAcademicYear[0]) + 1).ToString();
            }
            //if ((Month > 3 && Month < 1) && (Convert.ToInt32(splitAcademicYear[0]) == DateTime.Now.Year))
            if ((Month > 3 && IM.BudgetApprovedStatus!=1) && (Convert.ToInt32(splitAcademicYear[0]) == GlobalVariables.Year))
            {
                IM.IsEnabled = true;
            }
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
            ModelState.Clear();
            return View("InterimRev", IM);
        }


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(InterimRevision IM)
        {
            IM.EditEnabled = IM.SubGroupLedgerName;
            //var Month = DateTime.Now.Month;
            var Month = GlobalVariables.Month;
            var DivName = " ";
            var DivisionID = " ";
            if (User.Identity.Name.Equals("admin@test.com"))
            {
                DivName = IM.SelectedDivisionName.ToString();
                DivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }

            var splitAcademicYear = IM.SelectedAcademicYear.ToString().Split("-");
            int index = IM.SubGroupNameOrLedgerName.IndexOf(IM.SubGroupLedgerName);

            IM.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            //if ((Month > 3 && Month < 1) && (Convert.ToInt32(splitAcademicYear[0]) == DateTime.Now.Year))

            if (Month > 0 && Month < 4)
            {
                /*Year = DateTime.Now.Year - 1;*/
                splitAcademicYear[0] = (Convert.ToInt32(splitAcademicYear[0]) + 1).ToString();
            }
            if ((Month > 3 && IM.BudgetApprovedStatus!=1) && (Convert.ToInt32(splitAcademicYear[0]) == GlobalVariables.Year))
              {
                IM.IsEnabled = true;
            }

            IM.Sectionss = _context.BudgetSections.ToList();
            //IM.Groupss = _context.BudgetGroups.ToList();
            IM.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            IM.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
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
