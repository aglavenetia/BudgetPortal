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
        //public int Year = DateTime.Now.Year;
        public TabsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var Year = DateTime.Now.Year;
            //var Year = 2022;
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
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID==LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == (Year-1)).ToList();
            if (username != "admin@test.com")
            {
                mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).Where(x => x.SectionNumber != Convert.ToInt32(0)).ToList();
            }
            else
            {
                mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();
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
            //ViewData["SelectedAcademicID"] = mymodel.AcademicYears;

            mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;
           //mymodel.SelectedAcademicYearID = mymodel.AcademicYears.Where(x => x.Selected.Equals(true)).Select(x => x.Value).FirstOrDefault();

            return View(mymodel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MultipleData MD, IFormCollection Form)
        {
            
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

                                //result.ACAndBWPropRECurrFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                result.ACAndBWPropRECurrFin = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);
                                //result.ACAndBWPropRENxtFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
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

                            //result.ACAndBWPropRECurrFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", SectionNumber, GroupNumber, i)]);
                            result.ACAndBWPropRECurrFin = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);
                            //result.ACAndBWPropRENxtFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", SectionNumber, GroupNumber, i)]);
                            result.ACAndBWPropRENxtFin = Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]);
                        }

                        _context.BudgetDetails.Update(result);
                        _context.SaveChanges();
                    }

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
               

                var DivisionID = _context.Division
                                             .Where(d => d.DivisionName == DivName)
                                             .Select(x => x.DivisionID).FirstOrDefault();
                    MD.Sectionss = _context.BudgetSections.ToList();
                    MD.Groupss = _context.BudgetGroups.ToList();
                    MD.SubGroupss = _context.BudgetSubGroups.ToList();
                    MD.Ledgerss = _context.BudgetLedgers.ToList();
                    MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == DivisionID)
                                        .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
                    MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                                        .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();
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

                //Save Budget Details to Database
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
                MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                 .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber != Convert.ToInt32(0)).ToList();
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
                                //dataModel.BudEstCurrFin = Convert.ToDecimal(Form[String.Concat("BudEstCurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.BudEstCurrFin = Convert.ToDecimal(MD.BudEstCurrFin[index]);
                                //dataModel.ActPrevFin = Convert.ToDecimal(Form[String.Concat("ActPrevFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]);
                                dataModel.ActPrevFin = Convert.ToDecimal(MD.ActPrevFin[index]);                                //dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(Form[String.Concat("ActCurrFinTill2ndQuart", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                //dataModel.RevEstCurrFin = Convert.ToDecimal(Form[String.Concat("RevEstCurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.RevEstCurrFin = Convert.ToDecimal(MD.RevEstCurrFin[index]);
                                //dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(Form[String.Concat("PerVarRevEstOverBudgEstCurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstCurrFin[index]);
                                //dataModel.BudgEstNexFin = Convert.ToDecimal(Form[String.Concat("BudgEstNexFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.BudgEstNexFin = Convert.ToDecimal(MD.BudgEstNexFin[index]);
                                //dataModel.Justification = Convert.ToString(Form[String.Concat("Justification", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.PerVarRevEstOverBudgEstNxtFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstNxtFin[index]);
                                dataModel.Justification = MD.Justification[index].ToString();
                                
                                dataModel.SectionNumber = Convert.ToInt32(SectionNumber);
                                dataModel.GroupNumber = GroupNumber;
                                dataModel.SubGroupNumber = SubGroups[i];
                                dataModel.LedgerNumber = Ledgers[j];

                                _context.BudgetDetails.Add(dataModel);
                                _context.SaveChanges();


                            }
                        }

                        else
                        {
                            int index = MD.SubGroupNameOrLedgerName.IndexOf(SubGroups[i]);

                            dataModel.DivisionID = Convert.ToInt32(SelectedDivisionID);
                            dataModel.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                            dataModel.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                            
                            //dataModel.BudEstCurrFin = Convert.ToDecimal(Form[String.Concat("BudEstCurrFin", SectionNumber, GroupNumber, i)]);
                            dataModel.BudEstCurrFin = Convert.ToDecimal(MD.BudEstCurrFin[index]);
                            //dataModel.ActPrevFin = Convert.ToDecimal(Form[String.Concat("ActPrevFin", SectionNumber, GroupNumber, i)]);
                            dataModel.ActPrevFin = Convert.ToDecimal(MD.ActPrevFin[index]);
                            //dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(Form[String.Concat("ActCurrFinTill2ndQuart", SectionNumber, GroupNumber, i)]);
                            dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(MD.ActCurrFinTillsecondQuart[index]);
                            //dataModel.RevEstCurrFin = Convert.ToDecimal(Form[String.Concat("RevEstCurrFin", SectionNumber, GroupNumber, i)]);
                            dataModel.RevEstCurrFin = Convert.ToDecimal(MD.RevEstCurrFin[index]);
                            //dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(Form[String.Concat("PerVarRevEstOverBudgEstCurrFin", SectionNumber, GroupNumber, i)]);       
                            dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstCurrFin[index]);
                            //dataModel.BudgEstNexFin = Convert.ToDecimal(Form[String.Concat("BudgEstNexFin", SectionNumber, GroupNumber, i)]);
                            dataModel.BudgEstNexFin = Convert.ToDecimal(MD.BudgEstNexFin[index]);
                            //dataModel.Justification = Convert.ToString(Form[String.Concat("Justification", SectionNumber, GroupNumber, i)]);
                            dataModel.PerVarRevEstOverBudgEstNxtFin = Convert.ToDecimal(MD.PerVarRevEstOverBudgEstNxtFin[index]);
                            dataModel.Justification = MD.Justification[index].ToString();
                            

                            dataModel.SectionNumber = Convert.ToInt32(SectionNumber);
                            dataModel.GroupNumber = GroupNumber;
                            dataModel.SubGroupNumber = SubGroups[i];
                            dataModel.LedgerNumber = Convert.ToDecimal(0).ToString();
                            
                            _context.BudgetDetails.Add(dataModel);
                            _context.SaveChanges();

                        }
                    }

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

                    var FinalSubmitStatus = new BudgetdetailsStatus();
                    FinalSubmitStatus = _context.BudgetdetailsStatus
                                      .Where(b => (b.DivisionID == SelectedDivisionID)
                                               && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                               && (b.SectionNumber == 0)).FirstOrDefault();

                    if(FinalSubmitStatus == null)
                    {
                        FinalSubmitStatus.DivisionID = Convert.ToInt32(SelectedDivisionID);
                        FinalSubmitStatus.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                        FinalSubmitStatus.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                        FinalSubmitStatus.SectionNumber = Convert.ToInt32(0);
                        FinalSubmitStatus.GroupNumber = Convert.ToString(0);
                        FinalSubmitStatus.DelegateEditStatus = false;
                        FinalSubmitStatus.AdminEditStatus = true;

                        _context.BudgetdetailsStatus.Add(FinalSubmitStatus);
                        _context.SaveChanges();
                    }

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

            //var Month = DateTime.Now.Month;

            //if (Month > 0 && Month < 4)
            //{
            //    Year = DateTime.Now.Year - 1;
            //}
            
                var AcademicYear = String.Concat(Year, "-", (Year + 1));
                var mymodel = new MultipleData();
                mymodel.Sectionss = _context.BudgetSections.ToList();
                mymodel.Groupss = _context.BudgetGroups.ToList();
                mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
                mymodel.Ledgerss = _context.BudgetLedgers.ToList();
                mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                    .Where(x => x.FinancialYear1 == Year).ToList();
                mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.DivisionID == LoggedInDivisionID)
                                             .Where(x => x.FinancialYear1 == (Year - 1)).ToList();
                if (username != "admin@test.com")
                {
                    mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                    .Where(x => x.FinancialYear1 == Year).Where(x => x.SectionNumber != Convert.ToInt32(0)).ToList();
                }
                else
                {
                    mymodel.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == LoggedInDivisionID)
                                    .Where(x => x.FinancialYear1 == Year).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();
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
                mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;
                //mymodel.SelectedAcademicYearID = String.Concat(Year.ToString(),"-",(Year+1).ToString());
                if (username.Equals("admin@test.com"))
                {

                    mymodel.DivisionNames.Where(x => x.Text.Equals(Division)).Single().Selected = true;
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
                                        int index = MD.SubGroupNameOrLedgerName.IndexOf(Ledgers[j]);
                                        result.DivisionID = Convert.ToInt32(SelectedDivisionID);
                                        result.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                                        result.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                                        result.BudEstCurrFinACandBW = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", itemSections.SectionNo, itemsGroups.GroupNo, k, Ledgers[l])]);
                                        result.RevEstCurrFinACandBW = Convert.ToDecimal(0);
                                        result.BudEstNextFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", itemSections.SectionNo, itemsGroups.GroupNo, k, Ledgers[l])]);
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

                                k++;
                            }
                            j++;
                        }
                        i++;
                    }

                    var adminstatus = new BudgetdetailsStatus();

                    adminstatus.DivisionID = Convert.ToInt32(SelectedDivisionID);
                    adminstatus.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                    adminstatus.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                    adminstatus.SectionNumber = Convert.ToInt32(0);
                    adminstatus.GroupNumber = 0.ToString();
                    adminstatus.DelegateEditStatus = false;
                    adminstatus.AdminEditStatus = false;

                    _context.BudgetdetailsStatus.Add(adminstatus);
                    _context.SaveChanges();


                }
                /* for (int i = 0; i < SubGroups.Count(); i++)
                  {

                      var result = new BudgetDetailsApproved();


                      var LedgerStatus = _context.BudgetSubGroups
                                .Where(x => x.SubGroupNo.Equals(SubGroups[i]))
                                .Select(x => x.RequireInput).First();

                      var Ledgers = _context.BudgetLedgers
                            .Where(x => x.SubGroupNo.Equals(SubGroups[i]))
                            .Select(x => x.LedgerNo).ToList();

                      for (int l = 0; l < Ledgers.Count(); l++)
                      {
                          if (LedgerStatus)
                          {
                              int index = MD.SubGroupNameOrLedgerName.IndexOf(Ledgers[l]);
                              result.DivisionID = Convert.ToInt32(SelectedDivisionID);
                              result.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                              result.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                              //result.BudEstCurrFinACandBW = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", itemSections.SectionNo, itemsGroups.GroupNo, k, Ledgers[l])]);
                              result.BudEstCurrFinACandBW = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);
                              result.RevEstCurrFinACandBW = Convert.ToDecimal(0);
                              //result.BudEstNextFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", itemSections.SectionNo, itemsGroups.GroupNo, k, Ledgers[l])]);
                              result.SectionNumber = Convert.ToInt32(SectionNumber);
                              result.GroupNumber = GroupNumber;
                              result.SubGroupNumber = SubGroups[i];
                              result.LedgerNumber = Ledgers[l];

                              _context.BudgetDetailsApproved.Add(result);
                              _context.SaveChanges();


                          }

                          else
                          {
                              int index = MD.SubGroupNameOrLedgerName.IndexOf(SubGroups[i]);
                              result.DivisionID = Convert.ToInt32(SelectedDivisionID);
                              result.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                              result.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                              //result.BudEstCurrFinACandBW = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", itemSections.SectionNo, itemsGroups.GroupNo, k)]);
                              result.BudEstCurrFinACandBW = Convert.ToDecimal(MD.ACAndBWPropRECurrFin[index]);
                              result.RevEstCurrFinACandBW = Convert.ToDecimal(0);
                              //result.BudEstNextFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", itemSections.SectionNo, itemsGroups.GroupNo, k)]);
                              result.BudEstNextFin = Convert.ToDecimal(MD.ACAndBWPropRENxtFin[index]);
                              result.SectionNumber = Convert.ToInt32(SectionNumber);
                              result.GroupNumber = GroupNumber;
                              result.SubGroupNumber = SubGroups[i];
                              result.LedgerNumber = Ledgers[l];

                              _context.BudgetDetailsApproved.Add(result);
                              _context.SaveChanges();
                          }
                      }
                  }       
              }*/


                //_context.BudgetdetailsStatus.Update(adminstatus);
                //_context.SaveChanges();

                var DivisionID = _context.Division
                                           .Where(d => d.DivisionName == DivName)
                                           .Select(x => x.DivisionID).FirstOrDefault();
                MD.Sectionss = _context.BudgetSections.ToList();
                MD.Groupss = _context.BudgetGroups.ToList();
                MD.SubGroupss = _context.BudgetSubGroups.ToList();
                MD.Ledgerss = _context.BudgetLedgers.ToList();
                MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == DivisionID)
                                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).ToList();
                MD.Statuss = _context.BudgetdetailsStatus.Where(x => x.DivisionID == DivisionID)
                                    .Where(x => x.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0])).Where(x => x.SectionNumber == Convert.ToInt32(0)).ToList();
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
