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
            mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;
            //mymodel.SelectedAcademicYear = String.Concat(Year, "-", (Year + 1)).AsEnumerable().ToString();

            return View(mymodel);
        }

        //Save Budget Details to Database
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MultipleData MD, IFormCollection Form)
        {

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
            var SubGroupNumber = _context.BudgetSubGroups
                          .Where(x => x.GroupNo.Equals(GroupNumber))
                          .Select(x => x.SubGroupNo).ToList();
            

            for (int i = 0; i < SubGroupNumber.Count(); i++)
             {
                var LedgerNumber = _context.BudgetLedgers
                          .Where(x => x.SubGroupNo.Equals(SubGroupNumber[i]))
                          .Select(x => x.LedgerNo).ToList();
                  
                var dataModel = new BudgetDetails();
                
                    dataModel.DivisionID = Convert.ToInt32(SelectedDivisionID);
                    dataModel.FinancialYear1 = Convert.ToInt32(splitAcademicYear[0]);
                    dataModel.FinancialYear2 = Convert.ToInt32(splitAcademicYear[1]);
                    //dataModel.BudEstCurrFin = MD.BudEstCurrFin[i];
                    dataModel.BudEstCurrFin = Convert.ToDecimal(Form[String.Concat("BudEstCurrFin", SectionNumber, GroupNumber,i)]);
                    //dataModel.ActPrevFin = MD.ActPrevFin[i];
                    dataModel.ActPrevFin = Convert.ToDecimal(Form[String.Concat("ActPrevFin", SectionNumber, GroupNumber, i)]);
                    //dataModel.ActCurrFinTill2ndQuart = MD.ActCurrFinTill2ndQuart[i];
                    dataModel.ActCurrFinTill2ndQuart = Convert.ToDecimal(Form[String.Concat("ActCurrFinTill2ndQuart", SectionNumber, GroupNumber, i)]);
                    //dataModel.RevEstCurrFin = MD.RevEstCurrFin[i];
                    dataModel.RevEstCurrFin = Convert.ToDecimal(Form[String.Concat("RevEstCurrFin", SectionNumber, GroupNumber, i)]);
                    //dataModel.BudgEstNexFin = MD.BudgEstNexFin[i];
                    dataModel.BudgEstNexFin = Convert.ToDecimal(Form[String.Concat("BudgEstNexFin", SectionNumber, GroupNumber, i)]);
                    //dataModel.Justification = MD.Justification[i];
                    dataModel.Justification = Convert.ToString(Form[String.Concat("Justification", SectionNumber, GroupNumber, i)]);
                    dataModel.SectionNumber = Convert.ToInt32(SectionNumber);
                    dataModel.GroupNumber    = GroupNumber;
                    dataModel.SubGroupNumber = SubGroupNumber[i];
                    //foreach(var No in LedgerNumber)
                       //dataModel.LedgerNumber += No;

                _context.BudgetDetails.Add(dataModel);
                _context.SaveChanges();
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
            MD.AcademicYears.Where(x => x.Text.Equals(MD.SelectedAcademicYear.ToString())).Single().Selected = true;

            return View("Index",MD);
         }

        //Update Budget Details in Database
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDetailsAdmin(MultipleData MD, IFormCollection Form)
        {
            
            var username = User.Identity.Name;
            //var DivName = _context.Users
            //            .Where(x => x.UserName.Equals(username))
            //          .Select(x => x.BranchName).First();
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
                var result = _context.BudgetDetails.SingleOrDefault(b => (b.DivisionID == SelectedDivisionID)
                                                                  && (b.FinancialYear1.Equals(splitAcademicYear[0]))
                                                                  && (b.SectionNumber == SectionNumber)
                                                                  && (b.GroupNumber == GroupNumber)
                                                                  && (b.SubGroupNumber == SubGroups[i]));

              
                result.ACAndBWPropRECurrFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRECurrFin", SectionNumber, GroupNumber, i)]);
                
                result.ACAndBWPropRENxtFin = Convert.ToDecimal(Form[String.Concat("ACAndBWPropRENxtFin", SectionNumber, GroupNumber, i)]);
                

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
            MD.DivisionNames.Where(x => x.Text.Equals(MD.SelectedDivisionID.ToString())).Single().Selected = true;

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
