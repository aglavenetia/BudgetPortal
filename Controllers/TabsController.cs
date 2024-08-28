using Microsoft.AspNetCore.Mvc;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Bibliography;


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
            /*var Year = DateTime.Now.Year;*/
            var Year = 2024;
            
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
                /*Year = DateTime.Now.Year - 1;*/
                Year = 2024 - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            
            var mymodel = new MultipleData();
            mymodel.SelectedAcademicYear = AcademicYear;
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID==LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Filess = _context.BudgetFiles.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == (Year-1)).ToList();
            mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();

            int FinalApproved = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
            int SubmittedForApproval = (from a in mymodel.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
            int NumberOfGroups = (from a in mymodel.Groupss select a.GroupNo).Count();
            int PendingForChairmanApproval = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ACBWSubmission.Equals(true) select a.ACBWSubmission).Count();
            int PendingForFinCommitteeApproval = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ChairpersonApproval.Equals(true) select a.ChairpersonApproval).Count();
            int PendingForGenBodyApproval = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.FinCommitteeApproval.Equals(true) select a.FinCommitteeApproval).Count();
            int PendingForPublishing = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.GenBodyApproval.Equals(true) select a.GenBodyApproval).Count();

            if (FinalApproved > 0)
            {
                mymodel.ApprovedMessage = "* Budget Details Approved for the Financial Year " + mymodel.SelectedAcademicYear + "!!!";
                mymodel.WaitingForApprovalMessage = " ";
            }
            else if (PendingForPublishing > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending to be published.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForGenBodyApproval > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with General Body for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForFinCommitteeApproval > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with Financial Committee for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForChairmanApproval > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with ChairPerson for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && PendingForChairmanApproval == 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else
            {
                mymodel.ApprovedMessage = " ";
                mymodel.WaitingForApprovalMessage = " ";
            }





            mymodel.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            
            if (Month > 9 && mymodel.BudgetApprovedStatus != 1 && Year == 2024)
            {
                mymodel.IsEnabled = true;
            }

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
            var AcYear = mymodel.AcademicYears.Where(x => x.Selected.Equals(true)).Select(x => x.Text).FirstOrDefault();
            TempData["SelAcademicYear"] = JsonConvert.SerializeObject(AcYear);

            return View(mymodel);
        }

        //Deletes Files uploaded in the website
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(MultipleData MD)
        {
            var username = User.Identity.Name;
            var DivName = " ";
            var DivisionID = " ";

            var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(MD.SectionName))
                                  .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = _context.BudgetGroups
                     .Where(x => x.GroupName.Equals(MD.GroupName))
                     .Select(x => x.GroupNo).FirstOrDefault();

            var SubGroupNumber = _context.BudgetSubGroups
                     .Where(x => x.SubGroupNo.Equals(MD.SubGroupLedgerName))
                     .Select(x => x.SubGroupNo).FirstOrDefault();

            var LedgerNumber = _context.BudgetLedgers
                     .Where(x => x.LedgerNo.Equals(MD.SubGroupLedgerName))
                     .Select(x => x.LedgerNo).FirstOrDefault();

            if (LedgerNumber != null)
            {
                SubGroupNumber = _context.BudgetLedgers
                    .Where(x => x.LedgerNo.Equals(MD.SubGroupLedgerName))
                    .Select(x => x.SubGroupNo).FirstOrDefault();
            }

            if (User.Identity.Name.Equals("admin@test.com"))
            {
                DivName = MD.SelectedDivisionName.ToString();
                DivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }
            else
            {
                DivName = _context.Users
                            .Where(x => x.UserName.Equals(username))
                            .Select(x => x.BranchName).FirstOrDefault();
                DivisionID = _context.Division
                                    .Where(d => d.DivisionName == DivName)
                                    .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }

            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

            if(User.Identity.Name.Equals("admin@test.com"))
            {
                //code to be written
            }
            else
            {
            ModelState.Remove("ACAndBWPropRECurrFin");
            ModelState.Remove("ACAndBWPropRENxtFin");
            var result = new BudgetFiles();
            result = _context.BudgetFiles 
                                      .Where(b => (b.DivisionID == Convert.ToInt32(DivisionID))
                                               && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                               && (b.SectionNumber == SectionNumber)
                                               && (b.GroupNumber == GroupNumber)
                                               && (b.SubGroupNumber == SubGroupNumber)
                                               && (b.LedgerNumber == LedgerNumber)).FirstOrDefault();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            string fileNameWithPath = Path.Combine(path, result.FileName.ToString());
            //string FilePath = path.Concat();

            if (result.SupportingDocumentPath!= null)
            {
               System.IO.File.Delete(fileNameWithPath);
            }

            _context.BudgetFiles.Remove(result);
            _context.SaveChanges();
            MD.DelegateEditStatus = true;
            }

            MD.Sectionss = _context.BudgetSections.ToList();
            MD.Groupss = _context.BudgetGroups.ToList();
            MD.SubGroupss = _context.BudgetSubGroups.ToList();
            MD.Ledgerss = _context.BudgetLedgers.ToList();
            MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Filess = _context.BudgetFiles.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            //MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
            //                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();

            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

            MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();
            
            //var Month = DateTime.Now.Month;
            var Month = 10;

            if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
            {
                MD.IsEnabled = true;
            }

            MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
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

            return View("Index",MD);
        }

        
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(MultipleData MD)
        {

            var username = User.Identity.Name;
            var DivName = " ";
            var DivisionID = " ";
            int index = -1;
            if (User.Identity.Name.Equals("admin@test.com"))
            {
                DivName = MD.SelectedDivisionName.ToString();
                DivisionID = _context.Division
                         .Where(d => d.DivisionName == DivName)
                         .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }
            else
            {
                DivName = _context.Users
                        .Where(x => x.UserName.Equals(username))
                        .Select(x => x.BranchName).FirstOrDefault();
                DivisionID = _context.Division
                        .Where(d => d.DivisionName == DivName)
                        .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }

            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

            var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(MD.SectionName))
                                  .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = _context.BudgetGroups
                     .Where(x => x.GroupName.Equals(MD.GroupName))
                     .Select(x => x.GroupNo).FirstOrDefault();

            var SubGroupNumber = _context.BudgetSubGroups
                    .Where(x => x.SubGroupNo.Equals(MD.SubGroupLedgerName))
                    .Select(x => x.SubGroupNo).FirstOrDefault();

            var LedgerNumber = _context.BudgetLedgers
                    .Where(x => x.LedgerNo.Equals(MD.SubGroupLedgerName))
                    .Select(x => x.LedgerNo).FirstOrDefault();

            if (LedgerNumber != null)
            {
                SubGroupNumber = _context.BudgetLedgers
                           .Where(x => x.LedgerNo.Equals(MD.SubGroupLedgerName))
                           .Select(x => x.SubGroupNo).FirstOrDefault();
                index = MD.SubGroupNameOrLedgerName.IndexOf(LedgerNumber);

            }
            else
            { 
                index = MD.SubGroupNameOrLedgerName.IndexOf(MD.SubGroupLedgerName);
            }
            
            //MD.IsResponse = true;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

            ModelState.Remove("ACAndBWPropRECurrFin");
            ModelState.Remove("ACAndBWPropRENxtFin");
            ModelState.Remove("SubGroupLedgerName");
            if (MD.File is not null)
            { 
                    //get file extension
                    FileInfo fileInfo = new FileInfo(MD.File.FileName);
                    string[] Name = (MD.File.FileName).Split('.');
                    DateTime currentDateTime = DateTime.Now;
                    string fileName = Name[0] +"_"+ currentDateTime.ToString("dd-MM-yyyy_HH_mm_ss") + fileInfo.Extension;

                    string fileNameWithPath = Path.Combine(path, fileName);

           

                if (fileInfo.Extension.Equals(".pdf") && MD.File.Length < 1000000 )
                { 
                         using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                         {
                             MD.File.CopyTo(stream);
                         }
                    
                         //MD.IsSuccess = true;
                         //MD.Message = "Saved";

                         
                        

                         var result = new BudgetFiles();
            
                          result.DivisionID = Convert.ToInt32(DivisionID);
                          result.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                          result.FinancialYear2 = Convert.ToInt32(splitAcademicYear[0]) + Convert.ToInt32(1);
                          result.SectionNumber = Convert.ToInt32(SectionNumber);
                          result.GroupNumber = GroupNumber;
                          result.SubGroupNumber = SubGroupNumber;
                          result.LedgerNumber = LedgerNumber;
                          result.SupportingDocumentPath = "/Files/"+fileName;
                          result.FileName = fileName;

                          _context.BudgetFiles.Add(result);
                          _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("FileMessage_"+index, "Please Upload only PDF files with maximum size 1 MB");
                }
              } 
            else
              {
                ModelState.AddModelError("FileMessage_"+index, "Please add any PDF file with maximum size 1 MB");
               }
            MD.Sectionss = _context.BudgetSections.ToList();
            MD.Groupss = _context.BudgetGroups.ToList();
            MD.SubGroupss = _context.BudgetSubGroups.ToList();
            MD.Ledgerss = _context.BudgetLedgers.ToList();
            MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Filess = _context.BudgetFiles.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            //MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
            //                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();

            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

            MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            //var Month = DateTime.Now.Month;
            var Month = 10;

            if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
            {
                MD.IsEnabled = true;
            }

            MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
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

            //MD.DivisionNames.Where(x => x.Text.Equals(MD.SelectedDivisionName.ToString())).Single().Selected = true;

            return View("Index", MD);
        }

        

        //Displays details while changing values in DropDownList
        [HttpGet]
        public IActionResult GetDetails(int Year, String Division)
        {   
            
            var username = User.Identity.Name;
            var LoggedInDivisionID = 0;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).FirstOrDefault();

            if(username != "admin@test.com")
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
                                        .Where(x => x.FinancialYear1 == Convert.ToInt32(Year)).ToList();
                mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                             .Where(x => x.FinancialYear1 == (Year - 1)).ToList();
                
            mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                        .Where(x => x.FinancialYear1 == Convert.ToInt32(Year)).ToList();

            int FinalApproved = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
            int SubmittedForApproval = (from a in mymodel.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
            int NumberOfGroups = (from a in mymodel.Groupss select a.GroupNo).Count();
            int PendingForChairmanApproval = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ACBWSubmission.Equals(true) select a.ACBWSubmission).Count();
            int PendingForFinCommitteeApproval = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.ChairpersonApproval.Equals(true) select a.ChairpersonApproval).Count();
            int PendingForGenBodyApproval = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.FinCommitteeApproval.Equals(true) select a.FinCommitteeApproval).Count();
            int PendingForPublishing = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.GenBodyApproval.Equals(true) select a.GenBodyApproval).Count();

            if (FinalApproved > 0)
            {
                mymodel.ApprovedMessage = "* Budget Details Approved for the Financial Year " + mymodel.SelectedAcademicYear + "!!!";
                mymodel.WaitingForApprovalMessage = " ";
            }
            else if (PendingForPublishing > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending to be published.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForGenBodyApproval > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with General Body for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForFinCommitteeApproval > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with Financial Committee for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (PendingForChairmanApproval > 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with ChairPerson for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && PendingForChairmanApproval == 0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + mymodel.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else
            {
                mymodel.ApprovedMessage = " ";
                mymodel.WaitingForApprovalMessage = " ";
            }


            mymodel.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(Year)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

            //var Month = DateTime.Now.Month;
            var Month = 10;

            if (Month > 9 && mymodel.BudgetApprovedStatus != 1 && Year == 2024)
            {
                mymodel.IsEnabled = true;
            }

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
                   var AcYear = mymodel.AcademicYears.Where(x => x.Selected.Equals(true)).Select(x => x.Text).FirstOrDefault();
                    TempData["SelAcademicYear"] = JsonConvert.SerializeObject(AcYear);
            }
                catch(Exception ex)
                {
                    ModelState.AddModelError("SelectedAcademicYearID", "Please select any Academic Year");
                    
                }
                //mymodel.SelectedAcademicYearID = String.Concat(Year.ToString(),"-",(Year+1).ToString());
                if (username.Equals("admin@test.com"))
                {
                   try
                   {
                      mymodel.DivisionNames.Where(x => x.Text.Equals(Division)).Single().Selected = true;
                    var DiName = mymodel.DivisionNames.Where(x => x.Selected.Equals(true)).Select(x => x.Text).FirstOrDefault();
                    TempData["SelDivisionName"] = JsonConvert.SerializeObject(DiName);
                }
                   catch (Exception ex)
                   {
                       ModelState.AddModelError("SelectedDivisionID", "Please select any Division");

                   }
                }
                
            return View("Index", mymodel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult FinalSubmit(MultipleData MD, IFormCollection Form)
        {
            
                var username = User.Identity.Name;
                var DivName = MD.SelectedDivisionName.ToString();
                var SelectedDivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault();
                var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

                //Update Budget Details for Admin User
                if (User.Identity.Name.Equals("admin@test.com"))
                {

                /*ModelState.Remove("SectionName");
                ModelState.Remove("GroupName");
                ModelState.Remove("Message");
                ModelState.Remove("File");
                ModelState.Remove("FileName");
                ModelState.Remove("SubGroupLedgerName");
                if (ModelState.IsValid)
                  {*/
                    int i = 0;
                    foreach (var itemSections in _context.BudgetSections)
                    {
                        int j = 0;
                        foreach (var itemsGroups in _context.BudgetGroups.Where(d => d.SectionNo == itemSections.SectionNo))
                        {
                            int k = 0;
                            foreach (var itemsSubGroups in _context.BudgetSubGroups.Where(d => d.GroupNo.Equals(itemsGroups.GroupNo)))
                            {
                                var result = new BudgetDetailsApproved();


                                var LedgerStatus = _context.BudgetSubGroups
                                          .Where(x => x.SubGroupNo.Equals(itemsSubGroups.SubGroupNo))
                                          .Select(x => x.RequireInput).FirstOrDefault();

                                var Ledgers = _context.BudgetLedgers
                                      .Where(x => x.SubGroupNo.Equals(itemsSubGroups.SubGroupNo))
                                      .Select(x => x.LedgerNo).ToList();

                                if (LedgerStatus)
                                {
                                    for (int l = 0; l < Ledgers.Count(); l++)
                                    {
                                        int index = MD.SubGroupNameOrLedgerName.IndexOf(Ledgers[l]);
                                        result.DivisionID = Convert.ToInt32(SelectedDivisionID);
                                        result.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                                        result.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                                        result.BudEstCurrFinACandBW = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);
                                        result.RevEstCurrFinACandBW = Convert.ToDecimal(0);
                                        result.BudEstNextFin = Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]);
                                        result.SectionNumber = Convert.ToInt32(itemSections.SectionNo);
                                        result.GroupNumber = itemsGroups.GroupNo;
                                        result.SubGroupNumber = itemsSubGroups.SubGroupNo;
                                        result.LedgerNumber = Ledgers[l];

                                        _context.BudgetDetailsApproved.Add(result);
                                        _context.SaveChanges();


                                    }
                                }
                                else
                                {
                                    if(itemSections.SectionNo!=null && itemsGroups.GroupNo!=null && itemsSubGroups.SubGroupNo!=null)
                                    { 
                                    int index = MD.SubGroupNameOrLedgerName.IndexOf(itemsSubGroups.SubGroupNo);

                                    result.DivisionID = Convert.ToInt32(SelectedDivisionID);
                                    result.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                                    result.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                                    result.BudEstCurrFinACandBW = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);
                                    result.RevEstCurrFinACandBW = Convert.ToDecimal(0);
                                    result.BudEstNextFin = Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]);
                                    result.SectionNumber = Convert.ToInt32(itemSections.SectionNo);
                                    result.GroupNumber = itemsGroups.GroupNo;
                                    result.SubGroupNumber = itemsSubGroups.SubGroupNo;
                                    result.LedgerNumber = Convert.ToDecimal(0).ToString();

                                    _context.BudgetDetailsApproved.Add(result);
                                    _context.SaveChanges();
                                    }
                                }

                                k++;
                            }
                            j++;
                        }
                        i++;
                    }

                    var adminstatus = new BudgetdetailsStatus();

                    adminstatus = _context.BudgetdetailsStatus
                                      .Where(b => (b.DivisionID == SelectedDivisionID)
                                               && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                               && (b.SectionNumber == 0)).FirstOrDefault();


                   // adminstatus.DivisionID = Convert.ToInt32(SelectedDivisionID);
                   // adminstatus.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                   // adminstatus.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                   // adminstatus.SectionNumber = Convert.ToInt32(0);
                   // adminstatus.GroupNumber = 0.ToString();
                   // adminstatus.DelegateEditStatus = false;
                   adminstatus.GenBodyApproval = true;
                    adminstatus.AdminEditStatus = false;

                    _context.BudgetdetailsStatus.Update(adminstatus);
                    _context.SaveChanges();


                /*}*/

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
                //                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();

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
                else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && PendingForChairmanApproval == 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForChairmanApproval > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with ChairPerson for Approval.";
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

                if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
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

                return View("Index", MD);
            }

            return View("Index", MD);
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
            var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(MD.SectionName))
                                  .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = _context.BudgetGroups
                     .Where(x => x.GroupName.Equals(MD.GroupName))
                     .Select(x => x.GroupNo).FirstOrDefault();

            var SubGroupNumber = _context.BudgetSubGroups
                     .Where(x => x.SubGroupNo.Equals(MD.SubGroupLedgerName))
                     .Select(x => x.SubGroupNo).FirstOrDefault();


            var LedgerNumber = _context.BudgetLedgers
                     .Where(x => x.LedgerNo.Equals(MD.SubGroupLedgerName))
                     .Select(x => x.LedgerNo).FirstOrDefault();

            if (LedgerNumber != null)
            {
                SubGroupNumber = _context.BudgetLedgers
                    .Where(x => x.LedgerNo.Equals(MD.SubGroupLedgerName))
                    .Select(x => x.SubGroupNo).FirstOrDefault();
            }
            else
            {
                LedgerNumber = 0.ToString();
            }

            if (User.Identity.Name.Equals("admin@test.com"))
            {
                DivName = MD.SelectedDivisionName.ToString();
                DivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }
            else
            {
                DivName = _context.Users
                            .Where(x => x.UserName.Equals(username))
                            .Select(x => x.BranchName).FirstOrDefault();
                DivisionID = _context.Division
                                    .Where(d => d.DivisionName == DivName)
                                    .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }
            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

            if (username != "admin@test.com")
            {
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
                result.ActPrevFin = Convert.ToDecimal(0.0000);
                result.ActCurrFinTill2ndQuart = Convert.ToDecimal(0.0000);
                result.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(0.0000);
                result.PerVarRevEstOverBudgEstNxtFin = Convert.ToDecimal(0.0000);
                result.RevEstCurrFin = Convert.ToDecimal(0.0000);
                result.BudgEstNexFin = Convert.ToDecimal(0.0000);
                result.DelegateJustificationRevEst = " ";
                result.Justification = " ";

                result.HasDelegateSaved = false;
                result.CreatedDateTime = DateTime.Now;

                _context.BudgetDetails.Update(result);
                _context.SaveChanges();
            }
            else
            {
                ModelState.Remove("SubGroupLedgerName");
                ModelState.Remove("EditEnabled");
               
                var result = new BudgetDetails();
                result = _context.BudgetDetails
                                          .Where(b => (b.DivisionID == Convert.ToInt32(DivisionID))
                                                   && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                                   && (b.SectionNumber == SectionNumber)
                                                   && (b.GroupNumber == GroupNumber)
                                                   && (b.SubGroupNumber == SubGroupNumber)
                                                   && (b.LedgerNumber == LedgerNumber)).FirstOrDefault();
                result.ACAndBWPropRECurrFin = Convert.ToDecimal(0.00);
                result.ACBWJustificationRevEst = " ";
                result.ACAndBWPropRENxtFin = Convert.ToDecimal(0.00);
                result.ACBWJustificationBudgEstNxtFin = " ";
                
                result.HasAdminSaved = false;
                result.CreatedDateTime = DateTime.Now;

                _context.BudgetDetails.Update(result);
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
            MD.Ledgerss = _context.BudgetLedgers.ToList();
            MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Filess = _context.BudgetFiles.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
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

            ModelState.Clear();
            return View("Index", MD);
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

            if (username != "admin@test.com")
            {
                 SelectedDivisionID = _context.Division
                                 .Where(d => d.DivisionName == DivName)
                                 .Select(x => x.DivisionID).FirstOrDefault();
            }
            else
            {
                 SelectedDivisionID = Convert.ToInt32(MD.SelectedDivisionID);
            }
              /*var DivName = MD.SelectedDivisionName.ToString();
                var SelectedDivisionID = _context.Division
                                     .Where(d => d.DivisionName == DivName)
                                     .Select(x => x.DivisionID).FirstOrDefault();*/
            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
                var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(MD.SectionName))
                                  .Select(x => x.SectionNo).FirstOrDefault();
                var GroupNumber = _context.BudgetGroups
                         .Where(x => x.GroupName.Equals(MD.GroupName))
                         .Select(x => x.GroupNo).FirstOrDefault();

                var SubGroupNumber = _context.BudgetSubGroups
                             .Where(x => x.SubGroupNo.Equals(MD.SubGroupNameOrLedgerName[index]))
                             .Select(x => x.SubGroupNo).FirstOrDefault();
                var LedgerNumber = 0.ToString();

                if (SubGroupNumber is null)
                {
                    LedgerNumber = _context.BudgetLedgers
                                      .Where(x => x.LedgerNo.Equals(MD.SubGroupLedgerName))
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
            ModelState.Remove("HasAdminSaved");
            ModelState.Remove("HasDelegateSaved");

            //Saves Budget Finalised values of ACBW
            if (User.Identity.Name.Equals("admin@test.com"))
            {
                if (ModelState.IsValid)
                {
                    var result = new BudgetDetails();
                    result = _context.BudgetDetails
                                              .Where(b => (b.DivisionID == Convert.ToInt32(SelectedDivisionID))
                                                       && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                                       && (b.SectionNumber == SectionNumber)
                                                       && (b.GroupNumber == GroupNumber)
                                                       && (b.SubGroupNumber == SubGroupNumber)
                                                       && (b.LedgerNumber == LedgerNumber)).FirstOrDefault();
                    result.ACAndBWPropRECurrFin = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);

                    var SplitPerVarACBWRevEstOverBudgEstCurrFin = MD.PerVarACBWRevEstOverBudgEstCurrFin[index].ToString().Split("%");
                    result.PerVarACBWRevEstOverBudgEstCurrFin = Convert.ToDecimal(SplitPerVarACBWRevEstOverBudgEstCurrFin[0]);
                    
                    result.ACAndBWPropRENxtFin = Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]);
                    var SplitPerVarACBWRevEstOverBudgEstNxtFin = MD.PerVarACBWRevEstOverBudgEstNxtFin[index].ToString().Split("%");
                    result.PerVarACBWRevEstOverBudgEstNxtFin = Convert.ToDecimal(SplitPerVarACBWRevEstOverBudgEstNxtFin[0]);
                    try
                    {
                        if (MD.ACBWJustificationRevEst[index] is not null)
                           result.ACBWJustificationRevEst = MD.ACBWJustificationRevEst[index].ToString();
                        
                        else
                            result.ACBWJustificationRevEst = " ";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ACBWJustificationRevEst[" + index + "]", "Please enter Justification");
                    }
                    try
                    {
                        if (MD.ACBWJustificationBudgEstNxtFin[index] is not null)
                            result.ACBWJustificationBudgEstNxtFin = MD.ACBWJustificationBudgEstNxtFin[index].ToString();
                        else
                            result.ACBWJustificationBudgEstNxtFin = " ";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ACBWJustificationBudgEstNxtFin[" + index + "]", "Please enter Justification");
                    }

                    result.HasAdminSaved = true;
                    result.CreatedDateTime= DateTime.Now;

                    _context.BudgetDetails.Update(result);
                    _context.SaveChanges();
                }
            }
            //Saves Budget values for Delegates
            else
            {
                ModelState.Remove("SelectedDivisionID");
                ModelState.Remove("ACAndBWPropRECurrFin");
                ModelState.Remove("ACAndBWPropRENxtFin");
                ModelState.Remove("PerVarACBWRevEstOverBudgEstCurrFin");
                ModelState.Remove("PerVarACBWRevEstOverBudgEstNxtFin");
                ModelState.Remove("PerVarRevEstOverBudgEstCurrFin");
                ModelState.Remove("PerVarRevEstOverBudgEstNxtFin");

               
                if (ModelState.IsValid)
                {
                    var result = new BudgetDetails();
                    result = _context.BudgetDetails
                                              .Where(b => (b.DivisionID == Convert.ToInt32(SelectedDivisionID))
                                                       && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                                       && (b.SectionNumber == SectionNumber)
                                                       && (b.GroupNumber == GroupNumber)
                                                       && (b.SubGroupNumber == SubGroupNumber)
                                                       && (b.LedgerNumber == LedgerNumber)).FirstOrDefault();

                    if (result is null)
                    {
                        var nextresult = new BudgetDetails();
                        nextresult.DivisionID = Convert.ToInt32(SelectedDivisionID);
                        nextresult.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                        nextresult.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                        nextresult.SectionNumber = Convert.ToInt32(SectionNumber);
                        nextresult.GroupNumber = GroupNumber;
                        nextresult.SubGroupNumber = SubGroupNumber;
                        nextresult.LedgerNumber = LedgerNumber;

                        nextresult.BudEstCurrFin = Convert.ToDecimal(MD.BudEstCurrFin[index]);
                        nextresult.ActCurrFinTill2ndQuart = Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]);

                        nextresult.ActPrevFin = Convert.ToDecimal(MD.ActPrevFin[index]);

                        nextresult.RevEstCurrFin = Convert.ToDecimal(MD.RevEstCurrFin[index]);

                        var SplitPerVarRevEstOverBudgEstCurrFin = MD.PerVarRevEstOverBudgEstCurrFin[index].ToString().Split("%");
                        nextresult.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(SplitPerVarRevEstOverBudgEstCurrFin[0]);

                        nextresult.BudgEstNexFin = Convert.ToDecimal(MD.BudgEstNexFin[index]);

                        var SplitPerVarRevEstOverBudgEstNxtFin = MD.PerVarRevEstOverBudgEstNxtFin[index].ToString().Split("%");
                        nextresult.PerVarRevEstOverBudgEstNxtFin = Convert.ToDecimal(SplitPerVarRevEstOverBudgEstNxtFin[0]);
                        try
                        {
                            if (MD.Justification[index] is not null)
                                nextresult.Justification = MD.Justification[index].ToString();
                            else
                                nextresult.Justification = " ";
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("Justification[" + index + "]", "Please enter Justification");
                        }
                        try
                        {
                            if (MD.DelegateJustificationRevEst[index] is not null)
                                nextresult.DelegateJustificationRevEst = MD.DelegateJustificationRevEst[index].ToString();
                            else
                                nextresult.DelegateJustificationRevEst = " ";
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("DelegateJustificationRevEst[" + index + "]", "Please enter Justification");
                        }
                        nextresult.HasDelegateSaved = true;
                        nextresult.CreatedDateTime = DateTime.Now;
                        _context.BudgetDetails.Add(nextresult);
                        _context.SaveChanges();
                        
                    }
                    else
                    {
                        
                        result.BudEstCurrFin = Convert.ToDecimal(MD.BudEstCurrFin[index]);
                        result.ActCurrFinTill2ndQuart = Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]);

                        result.ActPrevFin = Convert.ToDecimal(MD.ActPrevFin[index]);

                        result.RevEstCurrFin = Convert.ToDecimal(MD.RevEstCurrFin[index]);

                        var splitPerVarRevEstOverBudgEstCurrFin = MD.PerVarRevEstOverBudgEstCurrFin[index].ToString().Split("%");
                        result.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(splitPerVarRevEstOverBudgEstCurrFin[0]);

                        result.BudgEstNexFin = Convert.ToDecimal(MD.BudgEstNexFin[index]);

                        var splitPerVarRevEstOverBudgEstNxtFin = MD.PerVarRevEstOverBudgEstNxtFin[index].ToString().Split("%");
                        result.PerVarRevEstOverBudgEstNxtFin = Convert.ToDecimal(splitPerVarRevEstOverBudgEstNxtFin[0]);
                        try
                        {
                            if (MD.Justification[index] is not null)
                                result.Justification = MD.Justification[index].ToString();
                            else
                                result.Justification = " ";
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("Justification[" + index + "]", "Please enter Justification");
                        }
                        try
                        {
                            if (MD.DelegateJustificationRevEst[index] is not null)
                                result.DelegateJustificationRevEst = MD.DelegateJustificationRevEst[index].ToString();
                            else
                                result.DelegateJustificationRevEst = " ";
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("DelegateJustificationRevEst[" + index + "]", "Please enter Justification");
                        }
                        result.HasDelegateSaved = true;
                        result.CreatedDateTime = DateTime.Now;
                        _context.BudgetDetails.Update(result);
                        _context.SaveChanges();
                        
                    }
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
            MD.Filess = _context.BudgetFiles.Where(x => x.DivisionID == SelectedDivisionID)
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == SelectedDivisionID)
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
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
            

            if (User.Identity.Name.Equals("admin@test.com"))
            {
                MD.DivisionNames.Where(x => x.Text.Equals(MD.SelectedDivisionName.ToString())).Single().Selected = true;
            }

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
            else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups  && PendingForChairmanApproval == 0)
            {
                MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                MD.ApprovedMessage = " ";
            }
            else
            {
                MD.ApprovedMessage = " ";
                MD.WaitingForApprovalMessage = " ";
            }



            return View("Index", MD);

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
            MD.Ledgerss = _context.BudgetLedgers.ToList();
            MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Filess = _context.BudgetFiles.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
            MD.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).ToList();
            MD.BudgetApprovedStatus = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                                .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).Where(x => x.AdminEditStatus == false).Select(x => x.AdminEditStatus).Count();

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

            return View("Index", MD);

        }


        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult FinancialCommiteeSubmission(MultipleData MD)
        {
            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

            var username = User.Identity.Name;
            var DivName = MD.SelectedDivisionName.ToString();
            var SelectedDivisionID = _context.Division
                                 .Where(d => d.DivisionName == DivName)
                                 .Select(x => x.DivisionID).FirstOrDefault();


            var SectionNumber = _context.BudgetSections
                             .Where(x => x.SectionName.Equals(MD.SectionName))
                             .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = _context.BudgetGroups
                        .Where(x => x.GroupName.Equals(MD.GroupName))
                        .Select(x => x.GroupNo).FirstOrDefault();
            var SubGroups = _context.BudgetSubGroups
                        .Where(x => x.GroupNo.Equals(GroupNumber))
                        .Select(x => x.SubGroupNo).ToList();

            //Update Budget Status for Admin User
            if (User.Identity.Name.Equals("admin@test.com"))
            {
                var Status = new BudgetdetailsStatus();
                Status = _context.BudgetdetailsStatus
                                  .Where(b => (b.DivisionID == SelectedDivisionID)
                                           && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                           && (b.SectionNumber == 0)
                                           && (b.GroupNumber == "0")).FirstOrDefault();

                Status.ChairpersonApproval = true;

                _context.BudgetdetailsStatus.Update(Status);
                _context.SaveChanges();
            }
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

            if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
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


            return View("Index", MD);
        }

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult GeneralBodySubmission(MultipleData MD)
        {
            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

            var username = User.Identity.Name;
            var DivName = MD.SelectedDivisionName.ToString();
            var SelectedDivisionID = _context.Division
                                 .Where(d => d.DivisionName == DivName)
                                 .Select(x => x.DivisionID).FirstOrDefault();


            var SectionNumber = _context.BudgetSections
                             .Where(x => x.SectionName.Equals(MD.SectionName))
                             .Select(x => x.SectionNo).FirstOrDefault();
            var GroupNumber = _context.BudgetGroups
                        .Where(x => x.GroupName.Equals(MD.GroupName))
                        .Select(x => x.GroupNo).FirstOrDefault();
            var SubGroups = _context.BudgetSubGroups
                        .Where(x => x.GroupNo.Equals(GroupNumber))
                        .Select(x => x.SubGroupNo).ToList();

            //Update Budget Status for Admin User
            if (User.Identity.Name.Equals("admin@test.com"))
            {
                var Status = new BudgetdetailsStatus();
                Status = _context.BudgetdetailsStatus
                                  .Where(b => (b.DivisionID == SelectedDivisionID)
                                           && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                           && (b.SectionNumber == 0)
                                           && (b.GroupNumber == "0")).FirstOrDefault();

                Status.FinCommitteeApproval = true;

                _context.BudgetdetailsStatus.Update(Status);
                _context.SaveChanges();
            }
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

            if (Month > 9 && MD.BudgetApprovedStatus != 1 && Convert.ToInt32(splitAcademicYear[0]) == 2024)
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


            return View("Index", MD);
        }

    }
}
