using Microsoft.AspNetCore.Mvc;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;


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
            var Year = DateTime.Now.Year;
            
            var username = User.Identity.Name;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).First();
            var LoggedInDivisionID = _context.Division
                                   .Where(d => d.DivisionName == DivName)
                                   .Select(x => x.DivisionID).FirstOrDefault();
            var Month = DateTime.Now.Month;

            if(Month > 0 && Month < 4)
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
                                  .Select(x => x.SectionNo).First();
            var GroupNumber = _context.BudgetGroups
                     .Where(x => x.GroupName.Equals(MD.GroupName))
                     .Select(x => x.GroupNo).First();

            var SubGroupNumber = _context.BudgetSubGroups
                     .Where(x => x.SubGroupNo.Equals(MD.SubGroupLedgerName))
                     .Select(x => x.SubGroupNo).FirstOrDefault();

            var LedgerNumber = _context.BudgetLedgers
                     .Where(x => x.LedgerName.Equals(MD.SubGroupLedgerName))
                     .Select(x => x.LedgerNo).FirstOrDefault();

            if (LedgerNumber != null)
            {
                SubGroupNumber = _context.BudgetLedgers
                    .Where(x => x.LedgerName.Equals(MD.SubGroupLedgerName))
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
                            .Select(x => x.BranchName).First();
                DivisionID = _context.Division
                                    .Where(d => d.DivisionName == DivName)
                                    .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }

            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
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

            MD.PreviousYearAdminCount = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                 .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();

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
                        .Select(x => x.BranchName).First();
                DivisionID = _context.Division
                        .Where(d => d.DivisionName == DivName)
                        .Select(x => x.DivisionID).FirstOrDefault().ToString();
            }

            var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");

            var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(MD.SectionName))
                                  .Select(x => x.SectionNo).First();
            var GroupNumber = _context.BudgetGroups
                     .Where(x => x.GroupName.Equals(MD.GroupName))
                     .Select(x => x.GroupNo).First();

            var SubGroupNumber = _context.BudgetSubGroups
                    .Where(x => x.SubGroupNo.Equals(MD.SubGroupLedgerName))
                    .Select(x => x.SubGroupNo).FirstOrDefault();

            var LedgerNumber = _context.BudgetLedgers
                    .Where(x => x.LedgerName.Equals(MD.SubGroupLedgerName))
                    .Select(x => x.LedgerNo).FirstOrDefault();

            if (LedgerNumber != null)
            {
                SubGroupNumber = _context.BudgetLedgers
                           .Where(x => x.LedgerName.Equals(MD.SubGroupLedgerName))
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

            MD.PreviousYearAdminCount = _context.BudgetdetailsStatus.Where(x => x.DivisionID == Convert.ToInt32(DivisionID))
                 .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();

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

        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MultipleData MD)
        {
            Boolean valid = true;
            //Update Budget Details for Admin User
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
                                     .Select(x => x.SectionNo).First();
                    var GroupNumber = _context.BudgetGroups
                                .Where(x => x.GroupName.Equals(MD.GroupName))
                                .Select(x => x.GroupNo).First();
                    var SubGroups = _context.BudgetSubGroups
                                .Where(x => x.GroupNo.Equals(GroupNumber))
                                .Select(x => x.SubGroupNo).ToList();

                ModelState.Remove("Message");
                ModelState.Remove("File");
                ModelState.Remove("FileName");
                ModelState.Remove("SubGroupLedgerName");
                
                if (ModelState.IsValid)
                {

                    

                    for (int i = 0; i < SubGroups.Count(); i++)
                    {
                        var result = new BudgetDetails();

                        

                        var LedgerStatus = _context.BudgetSubGroups
                                  .Where(x => x.SubGroupNo.Equals(SubGroups[i]))
                                  .Select(x => x.RequireInput).First();

                        var Ledgers = _context.BudgetLedgers
                              .Where(x => x.SubGroupNo.Equals(SubGroups[i]))
                              .Select(x => x.LedgerNo).ToList();

                        if (LedgerStatus)
                        {


                            for (int j = 0; j < Ledgers.Count(); j++)
                            {
                                int index = MD.SubGroupNameOrLedgerName.IndexOf(Ledgers[j]);
                                result = _context.BudgetDetails
                                      .Where(b => (b.DivisionID == SelectedDivisionID)
                                               && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                               && (b.SectionNumber == SectionNumber)
                                               && (b.GroupNumber == GroupNumber)
                                               && (b.SubGroupNumber == SubGroups[i])
                                               && (b.LedgerNumber == Ledgers[j])).FirstOrDefault();

                                if (Decimal.Compare(Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]), Convert.ToDecimal(0.00)) == 0)
                                {
                                    ModelState.AddModelError("ACAndBWPropRECurrFin[" + index + "]", "Please enter value");
                                    valid = false;
                                }
                                else
                                    result.ACAndBWPropRECurrFin = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);

                                if (Decimal.Compare(Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]), Convert.ToDecimal(0.00)) == 0)
                                {
                                    ModelState.AddModelError("ACAndBWPropRENxtFin[" + index + "]", "Please enter value");
                                    valid = false;
                                }
                                else
                                    result.ACAndBWPropRENxtFin = Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]);
                               
                            }
                        }
                        else
                        {
                            int index = MD.SubGroupNameOrLedgerName.IndexOf(SubGroups[i]);
                            

                            result = _context.BudgetDetails
                                      .Where(b => (b.DivisionID == SelectedDivisionID)
                                               && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                               && (b.SectionNumber == SectionNumber)
                                               && (b.GroupNumber == GroupNumber)
                                               && (b.SubGroupNumber == SubGroups[i])).FirstOrDefault();

                            if(Decimal.Compare(Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]), Convert.ToDecimal(0.00)) == 0)
                            { 
                                ModelState.AddModelError("ACAndBWPropRECurrFin[" + index + "]", "Please enter value");
                                valid = false;
                            }
                            else
                               result.ACAndBWPropRECurrFin = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);

                            if (Decimal.Compare(Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]), Convert.ToDecimal(0.00)) == 0)
                            { 
                                ModelState.AddModelError("ACAndBWPropRENxtFin[" + index + "]", "Please enter value");
                                valid = false;
                            }

                            else
                                result.ACAndBWPropRENxtFin = Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]);
                        }

                        if(valid)
                        { 
                           _context.BudgetDetails.Update(result);
                           _context.SaveChanges();
                        }
                    }
                    if (valid)
                    {
                        var Status = new BudgetdetailsStatus();
                        Status = _context.BudgetdetailsStatus
                                          .Where(b => (b.DivisionID == SelectedDivisionID)
                                                   && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                                   && (b.SectionNumber == SectionNumber)
                                                   && (b.GroupNumber == GroupNumber)).FirstOrDefault();

                        Status.AdminEditStatus = false;

                        _context.BudgetdetailsStatus.Update(Status);
                        _context.SaveChanges();
                    }

                    
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
                    //MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                    // .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();
                    MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                            .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                int FinalApproved = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
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
                     .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();
                    
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

               // MD.IsResponse = true;
                //MD.IsSuccess = true;
                //MD.Message = "Budget Details Submitted successfully";



                return View("Index", MD);
                  
                }

                //Add Budget Details to Database for a Delegate
                else
                {
                
                //if (ModelState.IsValid)
                //{
                    var username = User.Identity.Name;
                    var DivName = _context.Users
                             .Where(x => x.UserName.Equals(username))
                             .Select(x => x.BranchName).First();
                    var SelectedDivisionID = _context.Division
                                      .Where(d => d.DivisionName == DivName)
                                      .Select(x => x.DivisionID).FirstOrDefault();
                    var splitAcademicYear = MD.SelectedAcademicYear.ToString().Split("-");
                    var SectionNumber = _context.BudgetSections
                                  .Where(x => x.SectionName.Equals(MD.SectionName))
                                  .Select(x => x.SectionNo).First();
                    var GroupNumber = _context.BudgetGroups
                             .Where(x => x.GroupName.Equals(MD.GroupName))
                             .Select(x => x.GroupNo).First();
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

                MD.PreviousYearAdminCount = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                         .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();


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

                    ModelState.Remove("SelectedDivisionID");
                    ModelState.Remove("ACAndBWPropRECurrFin");
                    ModelState.Remove("ACAndBWPropRENxtFin");
                    ModelState.Remove("File");
                    ModelState.Remove("FileName");
                    ModelState.Remove("SubGroupLedgerName");


                    if (ModelState.IsValid)
                    {
                        for (int i = 0; i < SubGroups.Count(); i++)
                        {
                            var dataModel = new BudgetDetails();

                            var LedgerStatus = _context.BudgetSubGroups
                                  .Where(x => x.SubGroupNo.Equals(SubGroups[i]))
                                  .Select(x => x.RequireInput).First();

                            var Ledgers = _context.BudgetLedgers
                                  .Where(x => x.SubGroupNo.Equals(SubGroups[i]))
                                  .Select(x => x.LedgerNo).ToList();

                            if (LedgerStatus)
                            {
                                for (int j = 0; j < Ledgers.Count(); j++)
                                {
                                    int index = MD.SubGroupNameOrLedgerName.IndexOf(Ledgers[j]);

                                    dataModel.DivisionID = Convert.ToInt32(SelectedDivisionID);
                                    dataModel.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                                    dataModel.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);

                                    dataModel.BudEstCurrFin = Convert.ToDecimal(MD.BudEstCurrFin[index]);

                                    if (Decimal.Compare(Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]), Convert.ToDecimal(0.00)) == 0)
                                    {
                                        ModelState.AddModelError("ActCurrFinTillsecondQuart[" + index + "]", "Please enter Actual values till Second Quarter for " + Ledgers[j]);
                                        valid = false;
                                    }
                                    else
                                        dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]);

                                    if (Decimal.Compare(Convert.ToDecimal(MD.ActPrevFin[index]), Convert.ToDecimal(0.00)) == 0)
                                    {
                                        ModelState.AddModelError("ActPrevFin[" + index + "]", "Please enter Actual values of Prev. Financial Year for "+ Ledgers[j]);
                                        valid = false;
                                    }
                                    else
                                        dataModel.ActPrevFin = Convert.ToDecimal(MD.ActPrevFin[index]);

                                    dataModel.RevEstCurrFin = Convert.ToDecimal(MD.RevEstCurrFin[index]);
                                    dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstCurrFin[index]);

                                    dataModel.BudgEstNexFin = Convert.ToDecimal(MD.BudgEstNexFin[index]);

                                    dataModel.PerVarRevEstOverBudgEstNxtFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstNxtFin[index]);
                                    try
                                    {
                                        if (MD.Justification[index] is not null)
                                            dataModel.Justification = MD.Justification[index].ToString();
                                        else
                                            dataModel.Justification = " ";

                                    }
                                    catch (Exception ex)
                                    {
                                        ModelState.AddModelError("Justification[" + index + "]", "Please enter Justification");
                                    }
                                    dataModel.SectionNumber = Convert.ToInt32(SectionNumber);
                                    dataModel.GroupNumber = GroupNumber;
                                    dataModel.SubGroupNumber = SubGroups[i];
                                    dataModel.LedgerNumber = Ledgers[j];
                                

                                if(valid)
                                { 
                                    _context.BudgetDetails.Add(dataModel);
                                    _context.SaveChanges();
                                }

                            }
                            }

                            else
                            {
                                int index = MD.SubGroupNameOrLedgerName.IndexOf(SubGroups[i]);
                                
                                  dataModel.DivisionID = Convert.ToInt32(SelectedDivisionID);
                                  dataModel.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                                  dataModel.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);

                                  dataModel.BudEstCurrFin = Convert.ToDecimal(MD.BudEstCurrFin[index]);

                                  if (Decimal.Compare(Convert.ToDecimal(MD.ActPrevFin[index]), Convert.ToDecimal(0.00)) == 0)
                                  {
                                     ModelState.AddModelError("ActPrevFin[" + index + "]", "Please enter Actual values of Prev. Financial Year for " + SubGroups[i]);
                                     valid = false;
                                  }
                                 else
                                  dataModel.ActPrevFin = Convert.ToDecimal(MD.ActPrevFin[index]);

                                 if (Decimal.Compare(Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]), Convert.ToDecimal(0.00)) == 0)
                                 {
                                     ModelState.AddModelError("ActCurrFinTillsecondQuart[" + index + "]", "Please enter Actual values till Second Quarter for " + SubGroups[i]);
                                     valid = false;
                                 }
                                 else
                                    dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]);

                                  dataModel.RevEstCurrFin = Convert.ToDecimal(MD.RevEstCurrFin[index]);
                                  dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstCurrFin[index]);
                                  dataModel.BudgEstNexFin = Convert.ToDecimal(MD.BudgEstNexFin[index]);
                                  dataModel.PerVarRevEstOverBudgEstNxtFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstNxtFin[index]);
                                try 
                                { 
                                  if (MD.Justification[index] is not null)
                                    dataModel.Justification = MD.Justification[index].ToString();
                                  else
                                    dataModel.Justification = " ";
                                }
                                catch (Exception ex)
                                {
                                    ModelState.AddModelError("Justification[" + index + "]", "Please enter Justification");
                                }
                            
                                  dataModel.SectionNumber = Convert.ToInt32(SectionNumber);
                                  dataModel.GroupNumber = GroupNumber;
                                  dataModel.SubGroupNumber = SubGroups[i];
                                  dataModel.LedgerNumber = Convert.ToDecimal(0).ToString();
                                  
                                 if(valid)
                                 { 
                                   _context.BudgetDetails.Add(dataModel);
                                   _context.SaveChanges();
                                 }
                            }
                       }
                       
                        if(valid)
                        { 
                           var Status = new BudgetdetailsStatus();
                           Status.DivisionID = Convert.ToInt32(SelectedDivisionID);
                           Status.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                           Status.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                           Status.SectionNumber = SectionNumber;
                           Status.GroupNumber = GroupNumber;
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
                    int PendingForFinalSubmission = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();

                    if (FinalApproved > 0)
                    {
                        MD.ApprovedMessage = "Budget Details Approved for the Financial Year " + MD.SelectedAcademicYear + "!!!";
                        MD.WaitingForApprovalMessage = " ";
                    }
                    else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && SubmittedForApproval != 0)
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


                    MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                     .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                    }
                
                return View("Index", MD);
            }
        }

        //Displays details while changing values in DropDownList
        [HttpGet]
        public IActionResult GetDetails(int Year, String Division)
        {   
            
            var username = User.Identity.Name;
            var LoggedInDivisionID = 0;
            var DivName = _context.Users
                          .Where(x => x.UserName.Equals(username))
                          .Select(x => x.BranchName).First();

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
            int PendingForFinalSubmission = (from a in mymodel.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
            
            if (FinalApproved > 0)
            {
                mymodel.ApprovedMessage = "* Budget Details Approved for the Financial Year " + AcademicYear + "!!!";
                mymodel.WaitingForApprovalMessage = " ";
            }
            else if (FinalApproved == 0 && SubmittedForApproval == NumberOfGroups && SubmittedForApproval!=0)
            {
                mymodel.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + AcademicYear + " is pending with AC&BW for Approval.";
                mymodel.ApprovedMessage = " ";
            }
            else if(PendingForFinalSubmission > 0)
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
                     .Where(x => x.FinancialYear1 == Convert.ToInt32(Year - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();

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

                ModelState.Remove("SectionName");
                ModelState.Remove("GroupName");
                ModelState.Remove("Message");
                ModelState.Remove("File");
                ModelState.Remove("FileName");
                ModelState.Remove("SubGroupLedgerName");
                if (ModelState.IsValid)
                  {
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
                                          .Select(x => x.RequireInput).First();

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
                    adminstatus.AdminEditStatus = false;

                    _context.BudgetdetailsStatus.Update(adminstatus);
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
                //MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                //                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();

                MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                        .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();

                int FinalApproved = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(false) select a.AdminEditStatus).Count();
                int SubmittedForApproval = (from a in MD.Statuss where a.SectionNumber != 0 && !a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();
                int NumberOfGroups = (from a in MD.Groupss select a.GroupNo).Count();
                int PendingForFinalSubmission = (from a in MD.Statuss where a.SectionNumber == 0 && a.GroupNumber.Equals("0") && a.AdminEditStatus.Equals(true) select a.AdminEditStatus).Count();

                if (FinalApproved > 0)
                {
                    MD.ApprovedMessage = "* Budget Details Approved for the Financial Year " + MD.SelectedAcademicYear + "!!!";
                    MD.WaitingForApprovalMessage = " ";
                }
                else if (FinalApproved == 0 && SubmittedForApproval <= NumberOfGroups && SubmittedForApproval != 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with AC&BW for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else if (PendingForFinalSubmission > 0)
                {
                    MD.WaitingForApprovalMessage = "* Budget Details for the Financial Year " + MD.SelectedAcademicYear + " is pending with CMD for Approval.";
                    MD.ApprovedMessage = " ";
                }
                else
                {
                    MD.ApprovedMessage = " ";
                    MD.WaitingForApprovalMessage = " ";
                }



                MD.PreviousYearAdminCount = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                     .Where(x => x.FinancialYear1 == (Convert.ToInt32(splitAcademicYear[0]) - 1)).Where(x => x.SectionNumber == Convert.ToInt32(0)).Select(x => x.AdminEditStatus).Count();

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
    }
}
