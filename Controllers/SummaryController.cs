using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

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
            //var AcademicYear = String.Concat(Year, "-", (Year + 1));

            var AcademicYear = JsonConvert.DeserializeObject(TempData["SelAcademicYear"].ToString());
            var DName = JsonConvert.DeserializeObject(TempData["SelDivisionName"].ToString());

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
            mymodel.DivisionNames.Where(x => x.Text.Equals(DName)).Single().Selected = true;

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


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MultipleData MD)
        {
            Boolean valid = true;
            //Update Budget Status for Admin User
            if (User.Identity.Name.Equals("admin@test.com"))
            {

                var username = User.Identity.Name;
                var DivName = MD.SelectedDivisionName.ToString();
                var SelectedDivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault();
                var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

                var SectionNumber = _context.BudgetSections
                                 .Where(x => x.SectionName.Equals(MD.SectionName))
                                 .Select(x => x.SectionNo).FirstOrDefault();
                var GroupNumber = _context.BudgetGroups
                            .Where(x => x.GroupName.Equals(MD.GroupName))
                            .Select(x => x.GroupNo).FirstOrDefault();
                var SubGroups = _context.BudgetSubGroups
                            .Where(x => x.GroupNo.Equals(GroupNumber))
                            .Select(x => x.SubGroupNo).ToList();
                

                var DivisionID = _context.Division
                                             .Where(d => d.DivisionName == DivName)
                                             .Select(x => x.DivisionID).FirstOrDefault();
                MD.Sectionss = _context.BudgetSections.ToList();
                MD.Groupss = _context.BudgetGroups.ToList();
                MD.SubGroupss = _context.BudgetSubGroups.ToList();
                MD.Ledgerss = _context.BudgetLedgers.ToList();
                MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == DivisionID)
                                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
                MD.Filess = _context.BudgetFiles.Where(x => x.DivisionID == DivisionID)
                                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
                //MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                // .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();

                foreach (var Group in MD.Groupss)
                {
                    var Status = new BudgetdetailsStatus();
                    Status = _context.BudgetdetailsStatus
                                      .Where(b => (b.DivisionID == SelectedDivisionID)
                                               && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                               && (b.SectionNumber == Group.SectionNo)
                                               && (b.GroupNumber == Group.GroupNo)).FirstOrDefault();

                    Status.ACBWSubmission = true;

                    _context.BudgetdetailsStatus.Update(Status);
                    _context.SaveChanges();
                }



                MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                        .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                int FinalApproved = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
                int SubmittedForApproval = (from a in MD.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
                int NumberOfGroups = (from a in MD.Groupss select a.GroupNo).Count();
                int PendingForChairmanApproval = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ACBWSubmission.Equals(true) select a.ACBWSubmission).Count();
                int PendingForFinCommitteeApproval = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ChairpersonApproval.Equals(true) select a.ChairpersonApproval).Count();
                int PendingForGenBodyApproval = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.FinCommitteeApproval.Equals(true) select a.FinCommitteeApproval).Count();
                int PendingForPublishing = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.GenBodyApproval.Equals(true) select a.GenBodyApproval).Count();

                if (FinalApproved > 0)
                {
                    MD.ApprovedMessage = "* Budget Details Approved for the Financial Year " + MD.SelectedAcademicYear + "!!!";
                    MD.WaitingForApprovalMessage = " ";
                }
                else if (PendingForPublishing > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending to be published.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForGenBodyApproval > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with General Body for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForFinCommitteeApproval > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with Financial Committee for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForChairmanApproval > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with ChairPerson for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && PendingForChairmanApproval == 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else
                {
                    MD.ApprovedMessage = " ";
                    MD.WaitingForApprovalMessage = " ";
                }




                MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

                //var Month = DateTime.Now.Month;
                var Month = 10;

                if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2022)
                {
                    MD.IsEnabled = true;
                }

                MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == DivisionID)
                                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
                MD.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                        new SelectListItem()
                        {
                            Selected = false,
                            Text = x.DivisionName,
                            Value = x.DivisionID.ToString()

                        }).ToList();

                MD.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.Year1 + "-" + x.Year2,
                        Value = x.Id.ToString()

                    }).ToList();
                MD.AcademicYears.Where(x => x.Text.Equals(MD.SelectedAcademicYear.ToString())).Single().Selected = true;

                MD.DivisionNames.Where(x => x.Text.Equals(MD.SelectedDivisionName.ToString())).Single().Selected = true;



                return View("Summary", MD);

            }

            //Update Budget Status in Database for a Delegate
            else
            {


                var username = User.Identity.Name;
                var DivName = _context.Users
                         .Where(x => x.UserName.Equals(username))
                         .Select(x => x.BranchName).FirstOrDefault();
                var SelectedDivisionID = _context.Division
                                  .Where(d => d.DivisionName == DivName)
                                  .Select(x => x.DivisionID).FirstOrDefault();
                var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
                var SectionNumber = _context.BudgetSections
                              .Where(x => x.SectionName.Equals(MD.SectionName))
                              .Select(x => x.SectionNo).FirstOrDefault();
                var GroupNumber = _context.BudgetGroups
                         .Where(x => x.GroupName.Equals(MD.GroupName))
                         .Select(x => x.GroupNo).FirstOrDefault();
                var SubGroups = _context.BudgetSubGroups
                         .Where(x => x.GroupNo.Equals(GroupNumber))
                         .Select(x => x.SubGroupNo).ToList();


                var LoggedInDivisionID = _context.Division
                                    .Where(d => d.DivisionName == DivName)
                                    .Select(x => x.DivisionID).FirstOrDefault();

                MD.Sectionss = _context.BudgetSections.ToList();
                MD.Groupss = _context.BudgetGroups.ToList();
                MD.SubGroupss = _context.BudgetSubGroups.ToList();
                MD.Ledgerss = _context.BudgetLedgers.ToList();
                MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                 .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
                MD.Filess = _context.BudgetFiles.Where(x => x.DivisionID == LoggedInDivisionID)
                                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
                //MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                //.Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber != Convert.ToInt32(0)).ToList();
                MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                            .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                            .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

                //var Month = DateTime.Now.Month;
                var Month = 10;

                if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2022)
                {
                    MD.IsEnabled = true;
                }


                MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                     .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();

                MD.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                     new SelectListItem()
                     {
                         Selected = false,
                         Text = x.DivisionName,
                         Value = x.DivisionID.ToString()

                     }).ToList();
                MD.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.Year1 + "-" + x.Year2,
                        Value = x.Id.ToString()

                    }).ToList();
                //ViewBag.SelectedAcademicYearID = MD.AcademicYears;

                MD.AcademicYears.Where(x => x.Text.Equals(MD.SelectedAcademicYear.ToString())).Single().Selected = true;

                foreach (var Group in MD.Groupss)
                {
                    var Status = new BudgetdetailsStatus();
                    Status.DivisionID = Convert.ToInt32(SelectedDivisionID);
                    Status.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                    Status.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                    Status.SectionNumber = Group.SectionNo;
                    Status.GroupNumber = Group.GroupNo;
                    Status.DelegateEditStatus = false;
                    Status.AdminEditStatus = true;

                    _context.BudgetdetailsStatus.Add(Status);
                    _context.SaveChanges();
                }

                MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                         .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                int FinalApproved = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
                int SubmittedForApproval = (from a in MD.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
                int NumberOfGroups = (from a in MD.Groupss select a.GroupNo).Count();
                int PendingForChairmanApproval = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ACBWSubmission.Equals(true) select a.ACBWSubmission).Count();
                int PendingForFinCommitteeApproval = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ChairpersonApproval.Equals(true) select a.ChairpersonApproval).Count();
                int PendingForGenBodyApproval = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.FinCommitteeApproval.Equals(true) select a.FinCommitteeApproval).Count();
                int PendingForPublishing = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.GenBodyApproval.Equals(true) select a.GenBodyApproval).Count();

                if (FinalApproved > 0)
                {
                    MD.ApprovedMessage = "* Budget Details Approved for the Financial Year " + MD.SelectedAcademicYear + "!!!";
                    MD.WaitingForApprovalMessage = " ";
                }
                else if (PendingForPublishing > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending to be published.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForGenBodyApproval > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with General Body for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForFinCommitteeApproval > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with Financial Committee for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForChairmanApproval > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with ChairPerson for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && PendingForChairmanApproval == 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else
                {
                    MD.ApprovedMessage = " ";
                    MD.WaitingForApprovalMessage = " ";
                }



                MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                     .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                /* } */

                return View("Summary", MD);
            }
        }
    }
}
