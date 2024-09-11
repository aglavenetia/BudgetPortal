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
            var username = User.Identity.Name;
            String Div = " ";
            var DivName = " ";
            var LoggedInDivisionID = 0;
            if (username.Equals("admin@test.com"))
            {
                 Div = (string)JsonConvert.DeserializeObject(TempData["SelDivisionName"].ToString());
                 LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == Div)
                                   .Select(x => x.DivisionID).FirstOrDefault();
            }
            else
            { 
              DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();
              LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == DivName)
                                   .Select(x => x.DivisionID).FirstOrDefault();
            }
            var AcademicYear = JsonConvert.DeserializeObject(TempData["SelAcademicYear"].ToString());
            TempData["SelAcademicYear"] = AcademicYear;
            var SplitAcYear = AcademicYear.ToString().Split("-");
            var Year = Convert.ToInt32(SplitAcYear[0]);
            

            //var Month = DateTime.Now.Month;
            var Month = 10;
            if (Month > 0 && Month < 4)
            {
                //Year = DateTime.Now.Year - 1;
                Year = Year - 1;
            }
            //var AcademicYear = String.Concat(Year, "-", (Year + 1));

            //GetDetails(Year, Div.ToString());

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
            mymodel.LoggedInDivID = _context.Division.Where(x => x.DivisionID == LoggedInDivisionID).ToList();
            if (username.Equals("admin@test.com"))
            {
                
                mymodel.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                        new SelectListItem()
                        {
                            Selected = false,
                            Text = x.DivisionName,
                            Value = x.DivisionID.ToString()

                        }).ToList();
                mymodel.DivisionNames.Where(x => x.Text.Equals(Div)).Single().Selected = true;
            }
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
            mymodel.LoggedInDivID = _context.Division.Where(x => x.DivisionID == LoggedInDivisionID).ToList();

            if (username.Equals("admin@test.com"))
            {
                mymodel.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                        new SelectListItem()
                        {
                            Selected = false,
                            Text = x.DivisionName,
                            Value = x.DivisionID.ToString()

                        }).ToList();
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

                var Commts = new BudgetdetailsStatus();
                
                Commts.DivisionID = Convert.ToInt32(SelectedDivisionID);
                Commts.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                Commts.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                Commts.SectionNumber = 0;
                Commts.GroupNumber = "AddCmts";
                Commts.AdditionalComments = MD.Remarks[0];
                Commts.IsHeadApproved = MD.IsChecked;
                Commts.eoffFileNo = MD.eOfficeFileNumber;

                _context.BudgetdetailsStatus.Add(Commts);
                _context.SaveChanges();

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

                MD.LoggedInDivID = _context.Division.Where(x => x.DivisionID == LoggedInDivisionID).ToList();

                MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                     .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                /* } */

                return View("Summary", MD);
            }
        }


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult BudgetDelete(MultipleData MD)
        {
            var username = User.Identity.Name;
            var DivName = " ";
            var DivisionID = " ";
            int index = MD.SubGroupNameOrLedgerName.IndexOf(MD.SubGroupLedgerName);
            
            var SelectedDivisionID = 0;
            

            if (username == "admin@test.com")
            {
                SelectedDivisionID = Convert.ToInt32(MD.SelectedDivisionID);
            }


            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
            var SectionNumber = _context.BudgetGroups
                                .Where(x => x.GroupNo.Equals(MD.SubGroupLedgerName))
                                .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = MD.SubGroupLedgerName.ToString();


            if (User.Identity.Name.Equals("admin@test.com"))
            {
                DivName = MD.SelectedDivisionName.ToString();
                DivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }
            

            if (username == "admin@test.com")
            {
                ModelState.Remove("SubGroupLedgerName");
                ModelState.Remove("EditEnabled");

                var result = new BudgetdetailsStatus();
                result = _context.BudgetdetailsStatus
                                          .Where(b => (b.DivisionID == Convert.ToInt32(DivisionID))
                                                   && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                                   && (b.SectionNumber == SectionNumber)
                                                   && (b.GroupNumber == GroupNumber)).FirstOrDefault();
                result.Remarks = null;
                result.CreatedDateTime = DateTime.Now;
                _context.SaveChanges();

                ModelState.Clear();
            }

            MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            //var Month = DateTime.Now.Month;
            var Month = 10;

            if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
            {
                MD.IsEnabled = true;
            }

            MD.Sectionss = _context.BudgetSections.ToList();
            MD.Groupss = _context.BudgetGroups.ToList();
            MD.SubGroupss = _context.BudgetSubGroups.ToList();
            MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Ledgerss = _context.BudgetLedgers.ToList();
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

            ModelState.Clear();
            return View("Summary", MD);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MultipleData MD)
        {
            //Boolean valid = true;
            int index = MD.SubGroupNameOrLedgerName.IndexOf(MD.SubGroupLedgerName);
            var username = User.Identity.Name;
            var SelectedDivisionID = 0;
            var DivName = _context.Users
                      .Where(x => x.UserName.Equals(username))
                      .Select(x => x.BranchName).FirstOrDefault();

            if (username == "admin@test.com")
            {
               SelectedDivisionID = Convert.ToInt32(MD.SelectedDivisionID);
            }
            
            
            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
            var SectionNumber = _context.BudgetGroups
                                .Where(x => x.GroupNo.Equals(MD.SubGroupLedgerName))
                                .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = MD.SubGroupLedgerName.ToString();
            
            
            ModelState.Remove("SubGroupLedgerName");
            ModelState.Remove("EditEnabled");
            ModelState.Remove("HasAdminSaved");
            ModelState.Remove("HasDelegateSaved");
            ModelState.Remove("GroupName");
            ModelState.Remove("ActPrevFin");
            ModelState.Remove("SectionName");
            ModelState.Remove("BudEstCurrFin");
            ModelState.Remove("BudgEstNexFin");
            ModelState.Remove("InterimRevEst");
            ModelState.Remove("RevEstCurrFin");
            ModelState.Remove("ProvisionalRevEst");
            ModelState.Remove("ACAndBWPropRECurrFin");
            ModelState.Remove("ACAndBWPropRENxtFin");
            ModelState.Remove("ActCurrFinTillsecondQuart");
            ModelState.Remove("PerVarRevEstOverBudgEstCurrFin"); 
            ModelState.Remove("PerVarACBWRevEstOverBudgEstCurrFin");
            ModelState.Remove("PerVarRevEstOverBudgEstNxtFin");
            ModelState.Remove("PerVarACBWRevEstOverBudgEstNxtFin");

            //Saves Budget Finalised values of ACBW
            if (username == "admin@test.com")
            {
                if (ModelState.IsValid)
                {
                    var result = new BudgetdetailsStatus();
                    result = _context.BudgetdetailsStatus
                                              .Where(b => (b.DivisionID == Convert.ToInt32(SelectedDivisionID))
                                                       && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                                       && (b.SectionNumber == SectionNumber)
                                                       && (b.GroupNumber == GroupNumber)).FirstOrDefault();
                    
                    if(MD.Remarks[index].ToString() == "" || MD.Remarks[index] is null)
                    {
                        result.Remarks = "";
                    }
                    else
                    { 
                    result.Remarks = MD.Remarks[index].ToString();
                    }

                    result.CreatedDateTime = DateTime.Now;

                    //_context.BudgetdetailsStatus.Update(result);
                    _context.SaveChanges();
                }
            }
         
            var DivisionID = _context.Division
                             .Where(d => d.DivisionName == DivName)
                             .Select(x => x.DivisionID).FirstOrDefault();
            MD.EditEnabled = null;
            MD.Sectionss = _context.BudgetSections.ToList();
            MD.Groupss = _context.BudgetGroups.ToList();
            MD.SubGroupss = _context.BudgetSubGroups.ToList();
            MD.Ledgerss = _context.BudgetLedgers.ToList();
            MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == SelectedDivisionID)
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            //var Month = DateTime.Now.Month;
            var Month = 10;

            if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
            {
                MD.IsEnabled = true;
            }
            MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == SelectedDivisionID)
                                  .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == SelectedDivisionID)
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.DivisionNames = _context.Division.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.DivisionName,
                        Value = x.DivisionID.ToString()

                    }).ToList();
            MD.LoggedInDivID = _context.Division.Where(x => x.DivisionID == SelectedDivisionID).ToList();
            MD.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                new SelectListItem()
                {
                    Selected = false,
                    Text = x.Year1 + "-" + x.Year2,
                    Value = x.Id.ToString()

                }).ToList();
            MD.AcademicYears.Where(x => x.Text.Equals(MD.SelectedAcademicYear.ToString())).Single().Selected = true;


            if (User.Identity.Name.Equals("admin@test.com"))
            {
                MD.DivisionNames.Where(x => x.Text.Equals(MD.SelectedDivisionName.ToString())).Single().Selected = true;
            }

            return View("Summary", MD);

        }


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MultipleData MD)
        {
            MD.EditEnabled = MD.SubGroupLedgerName;

            var DivName = " ";
            var DivisionID = " ";
            if (User.Identity.Name.Equals("admin@test.com"))
            {
                DivName = MD.SelectedDivisionName.ToString();
                DivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }

            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
            int index = MD.SubGroupNameOrLedgerName.IndexOf(MD.SubGroupLedgerName);

            MD.Sectionss = _context.BudgetSections.ToList();
            MD.Groupss = _context.BudgetGroups.ToList();
            MD.SubGroupss = _context.BudgetSubGroups.ToList();
            MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
            MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();
            MD.Ledgerss = _context.BudgetLedgers.ToList();
            MD.LoggedInDivID = _context.Division.Where(x => x.DivisionID == Convert.ToInt32(DivisionID)).ToList();
            //var Month = DateTime.Now.Month;
            var Month = 10;

            if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
            {
                MD.IsEnabled = true;
            }

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

            return View("Summary", MD);

        }
    }
}
