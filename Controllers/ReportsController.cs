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
using BudgetPortal.Data.Migrations;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Data;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Routing.Constraints;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Spreadsheet;

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
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));
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
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));


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

            if (DivisionType == null)
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
                catch (Exception ex)
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
                        Text = x.ReportID.ToString().Equals("4") ? x.ReportName + " " + AcademicYear : x.ReportID.ToString().Equals("6") ? x.ReportName + " " + NextAcademicYear : x.ReportName,
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
            catch (Exception ex)
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
            var SelectedFileName = mymodel.ReportNames.Where(x => x.Selected == true).Select(x => x.Text).FirstOrDefault();
            return new Rotativa.AspNetCore.ViewAsPdf("PrintPDF", mymodel)
            {
                FileName = "Consolidated " + Report + " in " + AcademicYear + " for " + DivisionType + "_" + DateTime.Now + ".pdf",

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }


        public async Task<FileResult> ExportBudgetInExcel(int Year, String DivisionType, String Rep)
        {
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));
            var PreviousAcademicYear = String.Concat((Year - 1), "-", Year);

            var fileName = "Consolidated " + Rep + " in " + AcademicYear + " for " + DivisionType + "_" + DateTime.Now + ".xlsx";
            DataTable dataTable = new DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle.DataTable();
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
            mymodel.ReportNames.Where(x => x.Text.Equals(Rep)).Single().Selected = true;

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

            var SumBudEstCurrFin = 0.00;
            var SumActPrevFin = 0.00;
            var SumActCurrFinTill2ndQuart = 0.00;
            var SumRevEstCurrFin = 0.00;
            var SumBudgEstNexFin = 0.00;
            var SelectedAcademicYear = (from a in mymodel.AcademicYears where a.Selected == true select a.Text).FirstOrDefault();
            var ReportName = (from a in mymodel.ReportNames where a.Selected == true select a.Text).FirstOrDefault();
            var Report = (ReportName != null && @ReportName.Contains("Budget")) ? @ReportName : @ReportName + " " + @SelectedAcademicYear;
            //var NextAcademicYear = "";
            String[] SplitAcademicYear = new String[1];

            foreach (var inner in mymodel.Sectionss)
            {
                var inneridenti = String.Concat("Budget", @inner.SectionNo.ToString());


                var SectionSumBudEstCurrFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.BudEstCurrFin).Sum();
                var SectionSumActPrevFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ActPrevFin).Sum();
                var SectionSumActCurrFinTill2ndQuart = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ActCurrFinTill2ndQuart).Sum();
                var SectionSumACAndBWPropRECurrFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ACAndBWPropRECurrFin).Sum();
                var SectionSumPerVarRevEstOverBudgEstCurrFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.PerVarRevEstOverBudgEstCurrFin).Sum();
                var SectionSumACAndBWPropRENxtFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ACAndBWPropRENxtFin).Sum();
                var SectionSumPerVarRevEstOverBudgEstNxtFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.PerVarRevEstOverBudgEstNxtFin).Sum();

                var SelectedDivisionTypeName = (from a in mymodel.DivisionTypeNames where a.Selected == true select a.Text).FirstOrDefault();

                foreach (var item in mymodel.Groupss.Where(d => d.SectionNo == @inner.SectionNo))
                {
                    if (ReportName != null && ReportName.Equals("Headwise Consolidation"))
                    {
                        SumBudEstCurrFin = (double)(from a in mymodel.Detailss
                                                    where (a.GroupNumber.Equals(item.GroupNo))
                                                    select a.BudEstCurrFin).Sum();
                        SumActPrevFin = (double)(from a in mymodel.Detailss
                                                 where (a.GroupNumber.Equals(item.GroupNo))
                                                 select a.ActPrevFin).Sum();
                        SumActCurrFinTill2ndQuart = (double)(from a in mymodel.Detailss
                                                             where (a.GroupNumber.Equals(item.GroupNo))
                                                             select a.ActCurrFinTill2ndQuart).Sum();
                        SumRevEstCurrFin = (double)(from a in mymodel.Detailss
                                                    where (a.GroupNumber.Equals(item.GroupNo))
                                                    select a.ACAndBWPropRECurrFin).Sum();
                        SumBudgEstNexFin = (double)(from a in mymodel.Detailss
                                                    where (a.GroupNumber.Equals(item.GroupNo))
                                                    select a.ACAndBWPropRENxtFin).Sum();
                    }
                    else
                    {
                        SumBudEstCurrFin = (double)(from a in mymodel.Detailss
                                                    join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                    where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                    select a.BudEstCurrFin).Sum();
                        SumActPrevFin = (double)(from a in mymodel.Detailss
                                                 join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                 where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                 select a.ActPrevFin).Sum();
                        SumActCurrFinTill2ndQuart = (double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                             select a.ActCurrFinTill2ndQuart).Sum();
                        SumRevEstCurrFin = (double)(from a in mymodel.Detailss
                                                    join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                    where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                    select a.ACAndBWPropRECurrFin).Sum();
                        SumBudgEstNexFin = (double)(from a in mymodel.Detailss
                                                    join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                    where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                    select a.ACAndBWPropRENxtFin).Sum();
                    }

                    //var SelectedAcademicYear = (from a in mymodel.AcademicYears where a.Selected == true select a.Text).FirstOrDefault();
                    Report = (ReportName != null && @ReportName.Contains("Budget")) ? @ReportName : @ReportName + " " + @SelectedAcademicYear;
                    if (SelectedAcademicYear != null)
                    {
                        SplitAcademicYear = SelectedAcademicYear.Split("-");

                        NextAcademicYear = SplitAcademicYear[1] + "-" + (Convert.ToInt32(SplitAcademicYear[1]) + Convert.ToInt32(1));
                    }
                    var SumGroupReport = 0.00;

                    if (ReportName != null && ReportName.Equals("Actual"))
                    {
                        //SumGroupReport = (Double)(from a in mymodel.Detailss where (a.GroupNumber.Equals(@item.GroupNo) ) select a.ActPrevFin).Sum();
                        SumGroupReport = (Double)(from a in mymodel.Detailss
                                                  join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                  where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                  select a.ActPrevFin).Sum();
                    }
                    else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                    {
                        //SumGroupReport = (Double)(from a in mymodel.Detailss where (a.GroupNumber.Equals(@item.GroupNo)) select a.ActCurrFinTill2ndQuart).Sum();
                        SumGroupReport = (Double)(from a in mymodel.Detailss
                                                  join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                  where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                  select a.ActCurrFinTill2ndQuart).Sum();

                    }
                    else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                    {
                        //SumGroupReport = (Double)(from a in mymodel.Detailss where (a.GroupNumber.Equals(@item.GroupNo)) select a.ACAndBWPropRECurrFin).Sum();
                        SumGroupReport = (Double)(from a in mymodel.Detailss
                                                  join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                  where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                  select a.ACAndBWPropRECurrFin).Sum();
                    }

                    else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                    {
                        //SumGroupReport = (Double)(from a in mymodel.Detailss where (a.GroupNumber.Equals(@item.GroupNo)) select a.ACAndBWPropRENxtFin).Sum();
                        SumGroupReport = (Double)(from a in mymodel.Detailss
                                                  join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                  where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.GroupNumber.Equals(item.GroupNo))
                                                  select a.ACAndBWPropRENxtFin).Sum();

                    }



                    if (ReportName != null && ReportName.Equals("Headwise Consolidation"))
                    {

                        dataTable.Columns.AddRange(new DataColumn[]
                        {
                            new DataColumn("HEAD NO."),
                            new DataColumn("SUB HEAD NO."),
                            new DataColumn("HEAD NAME"),
                            new DataColumn("Budget Estimates" + SelectedAcademicYear),
                            new DataColumn("Actual" + PreviousAcademicYear),
                            new DataColumn("Actual upto 30.09."+Year),
                            new DataColumn("Revised Estimates"+ SelectedAcademicYear),
                            new DataColumn("Budget Estimates "+NextAcademicYear),
                        });

                        foreach (var Subs in mymodel.SubGroupss.Where(d => d.GroupNo.Equals(item.GroupNo)))
                        {

                            var BudEstCurrFinSum = (from a in mymodel.Detailss
                                                    where (a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                    select a.BudEstCurrFin).Sum();

                            var ActPrevFinSum = (from a in mymodel.Detailss
                                                 where (a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                 select a.ActPrevFin).Sum();

                            var ActCurrFinTill2ndQuartSum = (from a in mymodel.Detailss
                                                             where (a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                             select a.ActCurrFinTill2ndQuart).Sum();

                            var RevEstCurrFinSum = (from a in mymodel.Detailss
                                                    where (a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                    select a.ACAndBWPropRECurrFin).Sum();

                            var BudgEstNexFinSum = (from a in mymodel.Detailss
                                                    where (a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                    select a.ACAndBWPropRENxtFin).Sum();

                            var LedgerCount = (from a in mymodel.Ledgerss where a.SubGroupNo.Equals(Subs.SubGroupNo) select a.LedgerNo).Count();

                            DataRow row = dataTable.NewRow();

                            dataTable.Rows.Add(
                                Subs.SubGroupNo,
                                " ",
                                Subs.subGroupName,
                                BudEstCurrFinSum != null ? Convert.ToDecimal(BudEstCurrFinSum).ToString("F") : "-",
                                ActPrevFinSum != null ? Convert.ToDecimal(ActPrevFinSum).ToString("F") : "-",
                                ActCurrFinTill2ndQuartSum != null ? Convert.ToDecimal(ActCurrFinTill2ndQuartSum).ToString("F") : "-",
                                RevEstCurrFinSum != null ? Convert.ToDecimal(RevEstCurrFinSum).ToString("F") : "-",
                                BudgEstNexFinSum != null ? Convert.ToDecimal(BudgEstNexFinSum).ToString("F") : "-"
                                );

                            if (LedgerCount > 0)
                            {
                                foreach (var Ledgers in mymodel.Ledgerss.Where(d => d.SubGroupNo.Equals(Subs.SubGroupNo)))
                                {
                                    var BudEstCurrFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.BudEstCurrFin).Sum();
                                    var ActPrevFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ActPrevFin).Sum();
                                    var ActCurrFinTill2ndQuartLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ActCurrFinTill2ndQuart).Sum();
                                    var RevEstCurrFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ACAndBWPropRECurrFin).Sum();
                                    var BudgEstNexFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ACAndBWPropRENxtFin).Sum();

                                    dataTable.Rows.Add(
                                     " ",
                                      Ledgers.LedgerNo,
                                      Ledgers.LedgerName,
                                      BudEstCurrFinLedgerSum != null ? Convert.ToDecimal(BudEstCurrFinLedgerSum).ToString("F") : "-",
                                      ActPrevFinLedgerSum != null ? Convert.ToDecimal(ActPrevFinLedgerSum).ToString("F") : "-",
                                      ActCurrFinTill2ndQuartSum != null ? Convert.ToDecimal(ActCurrFinTill2ndQuartSum).ToString("F") : "-",
                                      RevEstCurrFinLedgerSum != null ? Convert.ToDecimal(RevEstCurrFinLedgerSum).ToString("F") : "-",
                                      BudgEstNexFinLedgerSum != null ? Convert.ToDecimal(BudgEstNexFinLedgerSum).ToString("F") : "-"
                                      );
                                }
                            }
                            TableCount++;
                        }
                        dataTable.Rows.Add(
                       " ",
                       "Total",
                       "Total",
                        SumBudEstCurrFin.ToString("F"),
                        SumActPrevFin.ToString("F"),
                        SumActCurrFinTill2ndQuart.ToString("F"),
                        SumRevEstCurrFin.ToString("F"),
                        SumBudgEstNexFin.ToString("F"));
                    }
                    else
                    {
                        dataTable.Columns.Add("HEAD NO.");
                        dataTable.Columns.Add("SUB HEAD NO.");
                        dataTable.Columns.Add("HEAD NAME");
                        dataTable.Columns.Add(@Report);
                        foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                        {
                            dataTable.Columns.Add(Regions.DivisionName);
                        }

                        DataRow row = dataTable.NewRow();
                        foreach (var Subs in mymodel.SubGroupss.Where(d => d.GroupNo.Equals(item.GroupNo)))
                        {
                            var details = (from a in mymodel.Detailss where a.SubGroupNumber.Equals(Subs.SubGroupNo) select a).FirstOrDefault();
                            var SumSubGroupReport = 0.00;

                            if (ReportName != null && ReportName.Equals("Actual"))
                            {
                                SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                             select a.ActPrevFin).Sum();

                            }
                            else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                            {
                                SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                             select a.ActCurrFinTill2ndQuart).Sum();
                            }
                            else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                            {
                                SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.SubGroupNumber.Equals(Subs.SubGroupNo))
                                                             select a.ACAndBWPropRECurrFin).Sum();
                            }
                            else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                            {
                                SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                             select a.ACAndBWPropRENxtFin).Sum();
                            }
                            var LedgerCount = (from a in mymodel.Ledgerss where a.SubGroupNo.Equals(Subs.SubGroupNo) select a.LedgerNo).Count();

                            row["HEAD NO."] = Subs.SubGroupNo;
                            row["SUB HEAD NO."] = " ";
                            row["HEAD NAME"] = Subs.subGroupName;
                            row[@Report] = SumSubGroupReport != 0.00 ? SumSubGroupReport.ToString("F") : "-";

                            foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                            {
                                var result = (from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(@Subs.SubGroupNo))) select a).FirstOrDefault();

                                if (ReportName != null && ReportName.Equals("Actual"))
                                {

                                    if (Subs.RequireInput)
                                    {
                                        var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(@Subs.SubGroupNo))) select a.ActPrevFin).Sum();
                                        row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F") : "-";
                                    }
                                    else
                                    {
                                        row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActPrevFin).ToString("F") : "-";
                                    }
                                }
                                else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                                {
                                    if (Subs.RequireInput)
                                    {
                                        var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(Subs.SubGroupNo))) select a.ActCurrFinTill2ndQuart).Sum();
                                        row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F") : "-";
                                    }
                                    else
                                    {
                                        row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActCurrFinTill2ndQuart).ToString("F") : "-";
                                    }
                                }
                                else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                                {
                                    if (Subs.RequireInput)
                                    {
                                        var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(@Subs.SubGroupNo))) select a.ACAndBWPropRECurrFin).Sum();
                                        row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F") : "-";
                                    }
                                    else
                                    {
                                        row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRECurrFin).ToString("F") : "-";
                                    }
                                }
                                else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                                {
                                    if (Subs.RequireInput)
                                    {
                                        var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(Subs.SubGroupNo))) select a.ACAndBWPropRENxtFin).Sum();
                                        row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F") : "-";
                                    }
                                    else
                                    {
                                        row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRENxtFin).ToString("F") : "-";
                                    }
                                }

                            }

                            dataTable.Rows.Add(row);

                            if (LedgerCount > 0)
                            {
                                foreach (var Ledgers in mymodel.Ledgerss.Where(d => d.SubGroupNo.Equals(Subs.SubGroupNo)))
                                {
                                    var BudEstCurrFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.BudEstCurrFin).Sum();
                                    var ActPrevFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ActPrevFin).Sum();
                                    var ActCurrFinTill2ndQuartLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ActCurrFinTill2ndQuart).Sum();
                                    var RevEstCurrFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ACAndBWPropRECurrFin).Sum();
                                    var BudgEstNexFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ACAndBWPropRENxtFin).Sum();

                                    DataRow row2 = dataTable.NewRow();

                                    row2["HEAD NO."] = " ";
                                    row2["SUB HEAD NO."] = Ledgers.LedgerNo;
                                    row2["HEAD NAME"] = Ledgers.LedgerName;
                                    row2[@Report] = SumSubGroupReport != 0.00 ? @SumSubGroupReport.ToString("F") : "-";

                                    foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                                    {
                                        var result = (from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.LedgerNumber.Equals(Ledgers.LedgerNo))) select a).FirstOrDefault();

                                        if (ReportName != null && ReportName.Equals("Actual"))
                                        {
                                            row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActPrevFin).ToString("F") : "-";
                                        }
                                        else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                                        {
                                            row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActCurrFinTill2ndQuart).ToString("F") : "-";
                                        }
                                        else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                                        {
                                            row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRECurrFin).ToString("F") : "-";
                                        }
                                        else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                                        {
                                            row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRENxtFin).ToString("F") : "-";
                                        }
                                    }

                                }
                            }
                            dataTable.Rows.Add(row2);

                            DataRow row1 = dataTable.NewRow();

                            row1["HEAD NO."] = " ";
                            row1["SUB HEAD NO."] = "Total";
                            row1["HEAD NAME"] = "Total";
                            row1[@Report] = SumGroupReport != 0.00 ? SumGroupReport.ToString("F") : "-";

                            foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                            {
                                if (ReportName != null && ReportName.Equals("Actual"))
                                {
                                    var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ActPrevFin).Sum();
                                    row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F") : "-";
                                }
                                else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                                {
                                    var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ActCurrFinTill2ndQuart).Sum();
                                    row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F") : "-";
                                }
                                else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                                {
                                    var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ACAndBWPropRECurrFin).Sum();
                                    row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F") : "-";
                                }
                                else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                                {
                                    var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ACAndBWPropRENxtFin).Sum();
                                    row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F") : "-";
                                }
                            }

                            dataTable.Rows.Add(row1);
                        }
                    }
                }
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
                       
        }
    }
}
