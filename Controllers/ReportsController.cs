using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BudgetPortal.Data;
using BudgetPortal.Entities;
using BudgetPortal.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using Rotativa;

namespace BudgetPortal.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Reports()
        {
            var Year = DateTime.Now.Year;
            var username = User.Identity.Name;
            
            var Month = DateTime.Now.Month;

            if (Month > 0 && Month < 4)
            {
                Year = DateTime.Now.Year - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year+1), "-", (Year + 2));
            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Divisionss = _context.Division.ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.FinancialYear1 == (Year - 1)).ToList();
            String[] DivisionTypes = _context.Division.Select(x => x.DivisionType).Distinct().ToArray();

            mymodel.DivisionTypeNames = new List<SelectListItem>();
            for (var i = 1;i<=DivisionTypes.Length;i++)
            {
                mymodel.DivisionTypeNames.Add(
                new SelectListItem
                {
                   Text = DivisionTypes[i-1],
                   Value = i.ToString()
                });
            }
            //mymodel.DivisionTypeNames.Add(new SelectListItem
            //{
            //    Text = "All",
            //    Value = (DivisionTypes.Length + 1).ToString()
            //});

            mymodel.DivisionTypeNames.Where(x => x.Text.Equals("Regional Office")).Single().Selected = true;
            //mymodel.SelectedDivisionTypeName = mymodel.DivisionTypeNames.Where(x => x.Selected == true).ToString();

            mymodel.ReportNames = _context.BudgetReports.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.ReportID.ToString().Equals("4") ? x.ReportName + " " + AcademicYear : x.ReportID.ToString().Equals("6") ? x.ReportName + " " + NextAcademicYear : x.ReportName,
                        Value = x.ReportID.ToString()

                    }).ToList();
            mymodel.ReportNames.Where(x => x.Text.Equals("Actual")).Single().Selected = true;
            //mymodel.SelectedReportName = mymodel.ReportNames.Where(x => x.Selected == true).ToString();

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

        //Displays details while changing values in DropDownList
        [HttpGet]
        public IActionResult GetDetails(int Year, String DivisionType, String Report)
        {
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year+1), "-", (Year + 2));


            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Divisionss = _context.Division.ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.FinancialYear1 == (Year - 1)).ToList();

            String[] DivisionTypes = _context.Division.Select(x => x.DivisionType).Distinct().ToArray();

            mymodel.DivisionTypeNames = new List<SelectListItem>();
            for (var i = 1; i <= DivisionTypes.Length; i++)
            {
                mymodel.DivisionTypeNames.Add(
                new SelectListItem
                {
                    Text = DivisionTypes[i - 1],
                    Value = i.ToString()
                });
            }
           // mymodel.DivisionTypeNames.Add(new SelectListItem
           // {
           //     Text = "All",
           //     Value = (DivisionTypes.Length + 1).ToString()
           // });

            if(DivisionType == null)
            {
                try
                {
                    mymodel.DivisionTypeNames.Where(x => x.Text.Equals("Regional Office")).Single().Selected = true;
                    //mymodel.SelectedDivisionTypeName = mymodel.DivisionTypeNames.Where(x => x.Selected == true).ToString();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("SelectedDivisionTypeID", "Please select any DivisionType");
                }
            }
            else
            {
                try
                {
                    mymodel.DivisionTypeNames.Where(x => x.Text.Equals(DivisionType)).Single().Selected = true;
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("SelectedDivisionTypeID", "Please select any DivisionType");
                }
            }
            try
            {
                mymodel.ReportNames = _context.BudgetReports.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.ReportID.ToString().Equals("4") ? x.ReportName+" "+ AcademicYear : x.ReportID.ToString().Equals("6") ? x.ReportName + " " + NextAcademicYear :  x.ReportName ,
                        Value = x.ReportID.ToString()

                    }).ToList();
            
                mymodel.ReportNames.Where(x => x.Text.Equals(Report)).Single().Selected = true;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("SelectedReportID", "Please select any Report");
            }

            //mymodel.SelectedReportName = mymodel.ReportNames.Where(x => x.Selected == true).ToString();
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
            catch(Exception ex) 
            {
                ModelState.AddModelError("SelectedAcademicYearID", "Please select any Academic Year");
            }
            //mymodel.SelectedAcademicYear = String.Concat(Year.ToString(),"-",(Year+1).ToString());
           
            return View("Reports", mymodel);
            
        }

        public ActionResult PrintPDF(int Year, String DivisionType, String Report)
        {
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));


            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.ToList();
            mymodel.Ledgerss = _context.BudgetLedgers.ToList();
            mymodel.Detailss = _context.BudgetDetails.Where(x => x.FinancialYear1 == Year).ToList();
            mymodel.Divisionss = _context.Division.ToList();
            mymodel.Approved = _context.BudgetDetailsApproved.Where(x => x.FinancialYear1 == (Year - 1)).ToList();

            mymodel.ReportNames = _context.BudgetReports.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.ReportID.ToString().Equals("4") ? x.ReportName + " " + AcademicYear : x.ReportID.ToString().Equals("6") ? x.ReportName + " " + NextAcademicYear : x.ReportName,
                        Value = x.ReportID.ToString()

                    }).ToList();
            mymodel.ReportNames.Where(x => x.Text.Equals(Report)).Single().Selected = true;

            mymodel.AcademicYears = _context.AcademicYears.AsEnumerable().Select(x =>
                    new SelectListItem()
                    {
                        Selected = false,
                        Text = x.Year1 + "-" + x.Year2,
                        Value = x.Id.ToString()

                    }).ToList();
            mymodel.AcademicYears.Where(x => x.Text.Equals(AcademicYear)).Single().Selected = true;

            String[] DivisionTypes = _context.Division.Select(x => x.DivisionType).Distinct().ToArray();

            mymodel.DivisionTypeNames = new List<SelectListItem>();
            for (var i = 1; i <= DivisionTypes.Length; i++)
            {
                mymodel.DivisionTypeNames.Add(
                new SelectListItem
                {
                    Text = DivisionTypes[i - 1],
                    Value = i.ToString()
                });
            }
            
            mymodel.DivisionTypeNames.Where(x => x.Text.Equals(DivisionType)).Single().Selected = true;

            //return View("PrintPDF",mymodel);

            /*string customSwitches = string.Format("--header-html  \"{0}\" " +
                               "--header-spacing \"0\" " +
                               "--footer-html \"{1}\" " +
                               "--footer-spacing \"10\" " +
                               "--footer-font-size \"10\" " +
                               "--header-font-size \"10\" ", header, footer);*/

            return new Rotativa.AspNetCore.ViewAsPdf("PrintPDF", mymodel)
            {
                FileName = "ReportGenerated.pdf",
                
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }

    }
}
