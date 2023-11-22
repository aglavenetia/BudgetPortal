using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            mymodel.SelectedAcademicYearID = mymodel.AcademicYears.Where(x => x.Selected.Equals(true)).Select(x => x.Value).FirstOrDefault();

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
                //if (ModelState.IsValid)
                //{ 
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
                            result = _context.BudgetDetails
                                  .Where(b => (b.DivisionID == SelectedDivisionID)
                                           && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                           && (b.SectionNumber == SectionNumber)
                                           && (b.GroupNumber == GroupNumber)
                                           && (b.SubGroupNumber == SubGroups[i])
                                           && (b.LedgerNumber == Ledgers[j])).FirstOrDefault();

                            result.ACAndBWPropRECurrFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                            result.ACAndBWPropRENxtFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                        }
                    }
                    else
                    {
                        result = _context.BudgetDetails
                                  .Where(b => (b.DivisionID == SelectedDivisionID)
                                           && (b.FinancialYear1 == Convert.ToInt32(splitAcademicYear[0]))
                                           && (b.SectionNumber == SectionNumber)
                                           && (b.GroupNumber == GroupNumber)
                                           && (b.SubGroupNumber == SubGroups[i])).FirstOrDefault();

                        result.ACAndBWPropRECurrFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", SectionNumber, GroupNumber, i)]);

                        result.ACAndBWPropRENxtFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", SectionNumber, GroupNumber, i)]);
                    }

                    _context.BudgetDetails.Update(result);
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
                    //ViewData["SelectedAcademicYearID"] = MD.AcademicYears;
                    //MD.SelectedAcademicYearID = ViewData["SelectedAcademicYearID"];
                    MD.DivisionNames.Where(x => x.Text.Equals(MD.SelectedDivisionName.ToString())).Single().Selected = true;
                    //ViewData["SelectedDivisionID"] = MD.DivisionNames;
                //}
                
                //else
                //{
                    //ModelState.AddModelError("BudgetDetails", "Please enter the mandatory details!");
                //}
                
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
                                dataModel.DivisionID = Convert.ToInt32(SelectedDivisionID);
                                dataModel.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                                dataModel.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                                dataModel.BudEstCurrFin = Convert.ToDecimal(Form[String.Concat("BudEstCurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.ActPrevFin = Convert.ToDecimal(Form[String.Concat("ActPrevFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(Form[String.Concat("ActCurrFinTill2ndQuart", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.RevEstCurrFin = Convert.ToDecimal(Form[String.Concat("RevEstCurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                //dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(Form[String.Concat("PerVarRevEstOverBudgEstCurrFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.BudgEstNexFin = Convert.ToDecimal(Form[String.Concat("BudgEstNexFin", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
                                dataModel.Justification = Convert.ToString(Form[String.Concat("Justification", SectionNumber, GroupNumber, i, Ledgers[j])][1]);
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
                            dataModel.DivisionID = Convert.ToInt32(SelectedDivisionID);
                            dataModel.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                            dataModel.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                            dataModel.BudEstCurrFin = Convert.ToDecimal(Form[String.Concat("BudEstCurrFin", SectionNumber, GroupNumber, i)]);
                            dataModel.ActPrevFin = Convert.ToDecimal(Form[String.Concat("ActPrevFin", SectionNumber, GroupNumber, i)]);
                            dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(Form[String.Concat("ActCurrFinTill2ndQuart", SectionNumber, GroupNumber, i)]);
                            dataModel.RevEstCurrFin = Convert.ToDecimal(Form[String.Concat("RevEstCurrFin", SectionNumber, GroupNumber, i)]);
                            //dataModel.PerVarRevEstOverBudgEstCurrFin = Convert.ToDecimal(Form[String.Concat("PerVarRevEstOverBudgEstCurrFin", SectionNumber, GroupNumber, i)]);       
                            dataModel.BudgEstNexFin = Convert.ToDecimal(Form[String.Concat("BudgEstNexFin", SectionNumber, GroupNumber, i)]);
                            dataModel.Justification = Convert.ToString(Form[String.Concat("Justification", SectionNumber, GroupNumber, i)]);
                            dataModel.SectionNumber = Convert.ToInt32(SectionNumber);
                            dataModel.GroupNumber = GroupNumber;
                            dataModel.SubGroupNumber = SubGroups[i];
                            dataModel.LedgerNumber = Convert.ToDecimal(0).ToString();
                            _context.BudgetDetails.Add(dataModel);
                            _context.SaveChanges();
                        }
                    }

                    var LoggedInDivisionID = _context.Division
                                        .Where(d => d.DivisionName == DivName)
                                        .Select(x => x.DivisionID).FirstOrDefault();
                    MD.Sectionss = _context.BudgetSections.ToList();
                    MD.Groupss = _context.BudgetGroups.ToList();
                    MD.SubGroupss = _context.BudgetSubGroups.ToList();
                    MD.Ledgerss = _context.BudgetLedgers.ToList();
                    MD.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
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
                    ViewBag.SelectedAcademicYearID = MD.AcademicYears;
                    MD.AcademicYears.Where(x => x.Text.Equals(MD.SelectedAcademicYear.ToString())).Single().Selected = true;
                    //MD.SelectedAcademicYearID = 0;
                    //MD.SelectedDivisionID = 0;
                //}
               return View("Index",MD);
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

            var Month = DateTime.Now.Month;

            if (Month > 0 && Month < 4)
            {
                Year = DateTime.Now.Year - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var mymodel = new MultipleData();
            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.DivisionID == LoggedInDivisionID)
                                .Where(x => x.FinancialYear1 == Year).ToList();
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
            //mymodel.SelectedAcademicYear = String.Concat(Year.ToString(),"-",(Year+1).ToString());
            if (username.Equals( "admin@test.com"))
            {
                mymodel.DivisionNames.Where(x => x.Text.Equals(Division)).Single().Selected = true;
            }
               return View("Index", mymodel);
        }
    }
}
