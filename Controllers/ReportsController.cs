﻿using Microsoft.AspNetCore.Mvc;
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
//using DocumentFormat.OpenXml.Drawing.Charts;
using System.Data;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Routing.Constraints;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace BudgetPortal.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReportsController> _logger;
        public ReportsController(ApplicationDbContext context, ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Reports()
        {
            //var Year = DateTime.Now.Year;
            var Year = GlobalVariables.Year;
            var username = User.Identity.Name;

            //var Month = DateTime.Now.Month;
            var Month = GlobalVariables.Month;
            if (Month > 0 && Month < 4)
            {
                //Year = DateTime.Now.Year - 1;
                Year = Year - 1;
            }
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));
            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
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
                        Text = x.ReportID.ToString().Equals("6") ? x.ReportName + " " + NextAcademicYear : x.ReportID.ToString().Equals("1") ? x.ReportName + " " + NextAcademicYear:x.ReportName,
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
            var AcYear = mymodel.AcademicYears.Where(x => x.Selected.Equals(true)).Select(x => x.Text).FirstOrDefault();
            TempData["SelAcademicYear"] = JsonConvert.SerializeObject(AcYear);

            return View(mymodel);
        }

        //Displays details while changing values in DropDownList
        [HttpGet]
        [Authorize]
        public IActionResult GetDetails(int Year, String DivisionType, String Report)
        {
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));


            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
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
                        Text = x.ReportID.ToString().Equals("1") ? x.ReportName + " " + NextAcademicYear : x.ReportID.ToString().Equals("6") ? x.ReportName + " " + NextAcademicYear : x.ReportName,
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

        [Authorize]
        public ActionResult PrintPDF(int Year, String DivisionType, String Report)
        {
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));

            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
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
                               "--footer-spacing \"1\" " +
                               "--footer-font-size \"1\" " +
                               "--header-font-size \"1\" ", header, footer);*/
            //var SelectedFileName = mymodel.ReportNames.Where(x => x.Selected == true).Select(x => x.Text).FirstOrDefault();
            //var ReportName = "";
            //var SelectedAcademicYear = mymodel.AcademicYears.Where(x => x.Selected == true).Select(x => x.Text).FirstOrDefault();
            
            //var SelectedDivisionTypeName = mymodel.DivisionTypeNames.Where(x => x.Selected == true).Select(x => x.Text).FirstOrDefault();

            if (Report != "Headwise Consolidation" && !Report.Contains("Consolidated"))
            {
                Report = "Consolidated " + Report ;
            }
            
            
            return new Rotativa.AspNetCore.ViewAsPdf("PrintPDF", mymodel)
            {
                
                 FileName = Report+ " in " + AcademicYear + " for " + DivisionType + "_" + DateTime.Now + ".pdf",
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }

        [Authorize]
        public async Task<FileResult> ExportBudgetInExcel(int Year, String DivisionType, String Rep)
        {

            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));
            var PreviousAcademicYear = String.Concat((Year - 1), "-", Year);

            if (Rep != "Headwise Consolidation" && !Rep.Contains("Consolidated"))
            {
                Rep = "Consolidated " + Rep + " for " + DivisionType;
            }
            var fileName = Rep + " in "+AcademicYear + "_" + DateTime.Now + ".xlsx";

            return (FileResult)GenerateExcel(fileName, Year, DivisionType, Rep);

        }

        [Authorize]
        private ActionResult GenerateExcel(String fileName, int Year, String DivisionType, String Rep)
        {

            
            var AcademicYear = String.Concat(Year, "-", (Year + 1));
            var NextAcademicYear = String.Concat((Year + 1), "-", (Year + 2));
            var PreviousAcademicYear = String.Concat((Year - 1), "-", Year);

            

            var mymodel = new ReportView();

            mymodel.Sectionss = _context.BudgetSections.ToList();
            mymodel.Groupss = _context.BudgetGroups.OrderBy(x => x.CreatedDateTime).ToList();
            mymodel.SubGroupss = _context.BudgetSubGroups.OrderBy(x => x.SubGroupNo).ToList();
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
            var Report = (Rep != null && Rep.Contains("Budget")) ? Rep : (Rep != null && Rep.Contains("Consolidated")) ? Rep: (Rep + " " + SelectedAcademicYear);
            //var NextAcademicYear = "";
            String[] SplitAcademicYear = new String[1];

            if (mymodel.Sectionss != null)
            {
                var SelectedDivisionTypeName = (from a in mymodel.DivisionTypeNames where a.Selected == true select a.Text).FirstOrDefault();

                DataTable dataTable = new DataTable();
                dataTable.TableName = Rep;

                DataTable dataTable1 = new DataTable();
                dataTable1.TableName = "Consolidated " + Rep;

                DataTable dataTable2 = new DataTable();
                dataTable2.TableName = Rep;

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
                    
                }
                else if(ReportName != null && ReportName.Contains("Consolidated"))
                {
                    dataTable2.Columns.Add("HEAD NO.");
                    dataTable2.Columns.Add("SUB HEAD NO.");
                    dataTable2.Columns.Add("HEAD NAME");
                    dataTable2.Columns.Add(@Report);
                    dataTable2.Columns.Add("HEADQUARTERS");
                    dataTable2.Columns.Add("ALL REGIONAL OFFICES");
                    dataTable2.Columns.Add("ALL COEs");
                    foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals("Unit")))
                    {
                        dataTable2.Columns.Add(Regions.DivisionName);
                    }
                }
                else
                {
                    dataTable1.Columns.Add("HEAD NO.");
                    dataTable1.Columns.Add("SUB HEAD NO.");
                    dataTable1.Columns.Add("HEAD NAME");
                    dataTable1.Columns.Add(@Report);
                    foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                    {
                        dataTable1.Columns.Add(Regions.DivisionName);
                    }

                    
                }

                foreach (var inner in mymodel.Sectionss)
                {
                    var inneridenti = String.Concat("Budget", @inner.SectionNo.ToString());


                    var SectionSumBudEstCurrFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.BudEstCurrFin).Sum();
                    var SectionSumActPrevFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ActPrevFin).Sum();
                    var SectionSumActCurrFinTill2ndQuart = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ActCurrFinTill2ndQuart).Sum();
                    var SectionSumACAndBWPropRECurrFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ACAndBWPropRECurrFin).Sum();
                    //var SectionSumPerVarRevEstOverBudgEstCurrFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.PerVarRevEstOverBudgEstCurrFin).Sum();
                    var SectionSumACAndBWPropRENxtFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.ACAndBWPropRENxtFin).Sum();
                    //var SectionSumPerVarRevEstOverBudgEstNxtFin = (from a in mymodel.Detailss where a.SectionNumber.Equals(@inner.SectionNo) select a.PerVarRevEstOverBudgEstNxtFin).Sum();

                    

                    if (mymodel.Groupss != null)
                    {

                        foreach (var item in mymodel.Groupss.Where(d => d.SectionNo == @inner.SectionNo))
                        {
                            //var SelectedAcademicYear = (from a in mymodel.AcademicYears where a.Selected == true select a.Text).FirstOrDefault();
                            Report = (Rep != null && Rep.Contains("Budget")) ? Rep : (Rep != null && Rep.Contains("Consolidated")) ? Rep : (Rep + " " + SelectedAcademicYear);
                            if (SelectedAcademicYear != null)
                           {
                            SplitAcademicYear = SelectedAcademicYear.Split("-");

                            NextAcademicYear = SplitAcademicYear[1] + "-" + (Convert.ToInt32(SplitAcademicYear[1]) + Convert.ToInt32(1));
                           }
                            var SumGroupReport = 0.00;
                            var HQSumGroupReport = 0.00;
                            var ROSumGroupReport = 0.00;
                            var COESumGroupReport = 0.00;
                            var UnitSumGroupReport = 0.00;

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


                            if (ReportName != null && ReportName.Equals("Consolidated Actual"))
                            {
                                //SumGroupReport = (Double)(from a in Model.Detailss where (a.GroupNumber.Equals(@item.GroupNo) ) select a.ActPrevFin).Sum();
                                SumGroupReport = (Double)(from a in mymodel.Detailss
                                                          where a.GroupNumber.Equals(@item.GroupNo)
                                                          select a.ActPrevFin).Sum();
                                HQSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("HeadQuarters") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ActPrevFin).Sum();
                                ROSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("Regional Office") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ActPrevFin).Sum();
                                COESumGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals("Centre of Excellence") && a.GroupNumber.Equals(@item.GroupNo))
                                                             select a.ActPrevFin).Sum();


                            }
                            else if (ReportName != null && ReportName.Equals("Consolidated Actual Half Year"))
                            {
                                //SumGroupReport = (Double)(from a in Model.Detailss where (a.GroupNumber.Equals(@item.GroupNo)) select a.ActCurrFinTill2ndQuart).Sum();
                                SumGroupReport = (Double)(from a in mymodel.Detailss
                                                          where a.GroupNumber.Equals(@item.GroupNo)
                                                          select a.ActCurrFinTill2ndQuart).Sum();
                                HQSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("HeadQuarters") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ActCurrFinTill2ndQuart).Sum();
                                ROSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("Regional Office") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ActCurrFinTill2ndQuart).Sum();
                                COESumGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals("Centre of Excellence") && a.GroupNumber.Equals(@item.GroupNo))
                                                             select a.ActCurrFinTill2ndQuart).Sum();
                            }
                            else if (ReportName != null && ReportName.Equals("Consolidated Revised Estimates"))
                            {
                                //SumGroupReport = (Double)(from a in Model.Detailss where (a.GroupNumber.Equals(@item.GroupNo)) select a.ACAndBWPropRECurrFin).Sum();
                                SumGroupReport = (Double)(from a in mymodel.Detailss
                                                          where a.GroupNumber.Equals(@item.GroupNo)
                                                          select a.ACAndBWPropRECurrFin).Sum();
                                HQSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("HeadQuarters") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ACAndBWPropRECurrFin).Sum();
                                ROSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("Regional Office") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ACAndBWPropRECurrFin).Sum();
                                COESumGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals("Centre of Excellence") && a.GroupNumber.Equals(@item.GroupNo))
                                                             select a.ACAndBWPropRECurrFin).Sum();
                            }
                            else if (ReportName != null && ReportName.Equals("Consolidated Budget Estimates " + @NextAcademicYear))
                            {
                                //SumGroupReport = (Double)(from a in Model.Detailss where (a.GroupNumber.Equals(@item.GroupNo)) select a.ACAndBWPropRENxtFin).Sum();
                                SumGroupReport = (Double)(from a in mymodel.Detailss
                                                          where a.GroupNumber.Equals(@item.GroupNo)
                                                          select a.ACAndBWPropRENxtFin).Sum();
                                HQSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("HeadQuarters") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ACAndBWPropRENxtFin).Sum();
                                ROSumGroupReport = (Double)(from a in mymodel.Detailss
                                                            join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                            where (b.DivisionType.Equals("Regional Office") && a.GroupNumber.Equals(@item.GroupNo))
                                                            select a.ACAndBWPropRENxtFin).Sum();
                                COESumGroupReport = (Double)(from a in mymodel.Detailss
                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                             where (b.DivisionType.Equals("Centre of Excellence") && a.GroupNumber.Equals(@item.GroupNo))
                                                             select a.ACAndBWPropRENxtFin).Sum();
                            }
                            if (ReportName != null && ReportName.Equals("Headwise Consolidation"))
                            {
                                dataTable.Rows.Add(item.GroupNo + " " + item.GroupName);
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

                                    //DataRow row = dataTable.NewRow();

                                    dataTable.Rows.Add(
                                        Subs.SubGroupNo,
                                        " ",
                                        Subs.subGroupName,
                                        BudEstCurrFinSum != null ? Convert.ToDecimal(BudEstCurrFinSum).ToString("F4") : "-",
                                        ActPrevFinSum != null ? Convert.ToDecimal(ActPrevFinSum).ToString("F4") : "-",
                                        ActCurrFinTill2ndQuartSum != null ? Convert.ToDecimal(ActCurrFinTill2ndQuartSum).ToString("F4") : "-",
                                        RevEstCurrFinSum != null ? Convert.ToDecimal(RevEstCurrFinSum).ToString("F4") : "-",
                                        BudgEstNexFinSum != null ? Convert.ToDecimal(BudgEstNexFinSum).ToString("F4") : "-"
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
                                              BudEstCurrFinLedgerSum != null ? Convert.ToDecimal(BudEstCurrFinLedgerSum).ToString("F4") : "-",
                                              ActPrevFinLedgerSum != null ? Convert.ToDecimal(ActPrevFinLedgerSum).ToString("F4") : "-",
                                              ActCurrFinTill2ndQuartSum != null ? Convert.ToDecimal(ActCurrFinTill2ndQuartSum).ToString("F4") : "-",
                                              RevEstCurrFinLedgerSum != null ? Convert.ToDecimal(RevEstCurrFinLedgerSum).ToString("F4") : "-",
                                              BudgEstNexFinLedgerSum != null ? Convert.ToDecimal(BudgEstNexFinLedgerSum).ToString("F4") : "-"
                                              );
                                        }
                                    }
                                }
                                dataTable.Rows.Add(
                            " ",
                            "Total",
                            "Total",
                             SumBudEstCurrFin.ToString("F4"),
                             SumActPrevFin.ToString("F4"),
                             SumActCurrFinTill2ndQuart.ToString("F4"),
                             SumRevEstCurrFin.ToString("F4"),
                             SumBudgEstNexFin.ToString("F4"));
                                

                            }

                            else if (ReportName != null && ReportName.Contains("Consolidated"))
                            {
                                dataTable2.Rows.Add(item.GroupNo + " " + item.GroupName);
                                foreach (var Subs in mymodel.SubGroupss.Where(d => d.GroupNo.Equals(@item.GroupNo)))
                                {
                                    DataRow row3 = dataTable2.NewRow();
                                    var details = (from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a).FirstOrDefault();
                                    //var SelectedAcademicYear = (from a in mymodel.AcademicYears where a.Selected == true select a.Text).FirstOrDefault();
                                    //var SplitAcademicYear = SelectedAcademicYear.Split("-");
                                    //var NextAcademicYear = SplitAcademicYear[1]+"-"+(Convert.ToInt32(SplitAcademicYear[1])+Convert.ToInt32(1));
                                    var SumSubGroupReport = 0.00;
                                    var HQSumSubGroupReport = 0.00;
                                    var ROSumSubGroupReport = 0.00;
                                    var COESumSubGroupReport = 0.00;
                                    var UnitSumSubGroupReport = 0.00;

                                    if (ReportName != null && ReportName.Equals("Consolidated Actual"))
                                    {
                                        //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ActPrevFin).Sum();
                                        SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                     where a.SubGroupNumber.Equals(@Subs.SubGroupNo)
                                                                     select a.ActPrevFin).Sum();
                                        HQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("HeadQuarters") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ActPrevFin).Sum();
                                        ROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("Regional Office") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ActPrevFin).Sum();
                                        COESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                        join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                        where (b.DivisionType.Equals("Centre of Excellence") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                        select a.ActPrevFin).Sum();
                                    }
                                    else if (ReportName != null && ReportName.Equals("Consolidated Actual Half Year"))
                                    {
                                        //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ActCurrFinTill2ndQuart).Sum();
                                        SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                     where a.SubGroupNumber.Equals(@Subs.SubGroupNo)
                                                                     select a.ActCurrFinTill2ndQuart).Sum();
                                        HQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("HeadQuarters") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ActCurrFinTill2ndQuart).Sum();
                                        ROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("Regional Office") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ActCurrFinTill2ndQuart).Sum();
                                        COESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                        join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                        where (b.DivisionType.Equals("Centre of Excellence") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                        select a.ActCurrFinTill2ndQuart).Sum();
                                    }
                                    else if (ReportName != null && ReportName.Equals("Consolidated Revised Estimates"))
                                    {
                                        //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ACAndBWPropRECurrFin).Sum();
                                        SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                     where a.SubGroupNumber.Equals(@Subs.SubGroupNo)
                                                                     select a.ACAndBWPropRECurrFin).Sum();
                                        HQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("HeadQuarters") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ACAndBWPropRECurrFin).Sum();
                                        ROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("Regional Office") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ACAndBWPropRECurrFin).Sum();
                                        COESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                        join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                        where (b.DivisionType.Equals("Centre of Excellence") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                        select a.ACAndBWPropRECurrFin).Sum();
                                    }
                                    else if (ReportName != null && ReportName.Equals("Consolidated Budget Estimates " + @NextAcademicYear))
                                    {
                                        //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ACAndBWPropRENxtFin).Sum();
                                        SumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                     where a.SubGroupNumber.Equals(@Subs.SubGroupNo)
                                                                     select a.ACAndBWPropRENxtFin).Sum();
                                        HQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("HeadQuarters") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ACAndBWPropRENxtFin).Sum();
                                        ROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                       join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                       where (b.DivisionType.Equals("Regional Office") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                       select a.ACAndBWPropRENxtFin).Sum();
                                        COESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                        join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                        where (b.DivisionType.Equals("Centre of Excellence") && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                        select a.ACAndBWPropRENxtFin).Sum();
                                    }

                                    var LedgerCount = (from a in mymodel.Ledgerss where a.SubGroupNo.Equals(@Subs.SubGroupNo) select a.LedgerNo).Count();



                                    row3["HEAD NO."] = Subs.SubGroupNo;
                                    row3["SUB HEAD NO."] = " ";
                                    row3["HEAD NAME"] = Subs.subGroupName;
                                    row3[@Report] = @SumSubGroupReport != 0.00 ? @SumSubGroupReport.ToString("F4") : "-";
                                    row3["HEADQUARTERS"] = @HQSumSubGroupReport != 0.00 ? @HQSumSubGroupReport.ToString("F4") : "-";
                                    row3["ALL REGIONAL OFFICES"] = @ROSumSubGroupReport != 0.00 ? @ROSumSubGroupReport.ToString("F4") : "-";
                                    row3["ALL COEs"] = @COESumSubGroupReport != 0.00 ? @COESumSubGroupReport.ToString("F4") : "-";


                                    foreach(var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals("Unit")))
                                    {
                                        if (ReportName != null && ReportName.Equals("Consolidated Actual"))
                                        {
                                            UnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(Regions.DivisionName) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                             select a.ACAndBWPropRENxtFin).Sum();

                                            row3[Regions.DivisionName] = @UnitSumSubGroupReport != 0.00 ? @UnitSumSubGroupReport.ToString("F4") : "-";

                                        }
                                        else if (ReportName != null && ReportName.Equals("Consolidated Actual Half Year"))
                                        {
                                            UnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(Regions.DivisionName) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                             select a.ActCurrFinTill2ndQuart).Sum();

                                            row3[Regions.DivisionName] = @UnitSumSubGroupReport != 0.00 ? @UnitSumSubGroupReport.ToString("F4") : "-";


                                        }
                                        else if (ReportName != null && ReportName.Equals("Consolidated Revised Estimates"))
                                        {
                                            UnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(Regions.DivisionName) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                             select a.ACAndBWPropRECurrFin).Sum();

                                            row3[Regions.DivisionName] = @UnitSumSubGroupReport != 0.00 ? @UnitSumSubGroupReport.ToString("F4") : "-";


                                        }
                                        else if (ReportName != null && ReportName.Equals("Consolidated Budget Estimates " + @NextAcademicYear))
                                        {
                                            UnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(Regions.DivisionName) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                             select a.ACAndBWPropRENxtFin).Sum();

                                            row3[Regions.DivisionName] = @UnitSumSubGroupReport != 0.00 ? @UnitSumSubGroupReport.ToString("F4") : "-";

                                        }

                                    }
                                    dataTable2.Rows.Add(row3);

                                    if(LedgerCount > 0)
                                    {
                                        foreach(var Ledgers in mymodel.Ledgerss.Where(d => d.SubGroupNo.Equals(@Subs.SubGroupNo)))
                                        {
                                            DataRow row4 = dataTable2.NewRow();
                                            var LedgerSumSubGroupReport = 0.00;
                                            var LedgerHQSumSubGroupReport = 0.00;
                                            var LedgerROSumSubGroupReport = 0.00;
                                            var LedgerCOESumSubGroupReport = 0.00;
                                            var LedgerUnitSumSubGroupReport = 0.00;

                                            if (ReportName != null && ReportName.Equals("Consolidated Actual"))
                                            {
                                                //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ActPrevFin).Sum();
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                   where (a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                   select a.ActPrevFin).Sum();
                                                LedgerHQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("HeadQuarters") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ActPrevFin).Sum();
                                                LedgerROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("Regional Office") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ActPrevFin).Sum();
                                                LedgerCOESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                      where (b.DivisionType.Equals("Centre of Excellence") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                      select a.ActPrevFin).Sum();
                                            }
                                            else if (ReportName != null && ReportName.Equals("Consolidated Actual Half Year"))
                                            {
                                                //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ActCurrFinTill2ndQuart).Sum();
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                   where (a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                   select a.ActCurrFinTill2ndQuart).Sum();
                                                LedgerHQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("HeadQuarters") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ActCurrFinTill2ndQuart).Sum();
                                                LedgerROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("Regional Office") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ActCurrFinTill2ndQuart).Sum();
                                                LedgerCOESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                      where (b.DivisionType.Equals("Centre of Excellence") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                      select a.ActCurrFinTill2ndQuart).Sum();
                                            }
                                            else if (ReportName != null && ReportName.Equals("Consolidated Revised Estimates"))
                                            {
                                                //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ACAndBWPropRECurrFin).Sum();
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                   where a.LedgerNumber.Equals(@Ledgers.LedgerNo)
                                                                                   select a.ACAndBWPropRECurrFin).Sum();
                                                LedgerHQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("HeadQuarters") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ACAndBWPropRECurrFin).Sum();
                                                LedgerROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("Regional Office") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ACAndBWPropRECurrFin).Sum();
                                                LedgerCOESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                      where (b.DivisionType.Equals("Centre of Excellence") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                      select a.ACAndBWPropRECurrFin).Sum();
                                            }
                                            else if (ReportName != null && ReportName.Equals("Consolidated Budget Estimates " + @NextAcademicYear))
                                            {
                                                //SumSubGroupReport = (Double)(from a in mymodel.Detailss where a.SubGroupNumber.Equals(@Subs.SubGroupNo) select a.ACAndBWPropRENxtFin).Sum();
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                   where (a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                   select a.ACAndBWPropRENxtFin).Sum();
                                                LedgerHQSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("HeadQuarters") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ACAndBWPropRECurrFin).Sum();
                                                LedgerROSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                     join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                     where (b.DivisionType.Equals("Regional Office") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                     select a.ACAndBWPropRECurrFin).Sum();
                                                LedgerCOESumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                      where (b.DivisionType.Equals("Centre of Excellence") && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                      select a.ACAndBWPropRECurrFin).Sum();
                                            }

                                            row4["HEAD NO."] = " ";
                                            row4["SUB HEAD NO."] = Ledgers.LedgerNo;
                                            row4["HEAD NAME"] = Ledgers.LedgerName;
                                            row4[@Report] = @LedgerSumSubGroupReport != 0.00 ? @LedgerSumSubGroupReport.ToString("F4") : "-";
                                            row4["HEADQUARTERS"] = @LedgerHQSumSubGroupReport != 0.00 ? @LedgerHQSumSubGroupReport.ToString("F4") : "-";
                                            row4["ALL REGIONAL OFFICES"] = @LedgerROSumSubGroupReport != 0.00 ? @LedgerROSumSubGroupReport.ToString("F4") : "-";
                                            row4["ALL COEs"] = @LedgerCOESumSubGroupReport != 0.00 ? @LedgerCOESumSubGroupReport.ToString("F4") : "-";

                                            foreach(var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals("Unit")))
                                            {
                                                if (ReportName != null && ReportName.Equals("Consolidated Actual"))
                                                {
                                                    LedgerUnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                           join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                           where (b.DivisionType.Equals(Regions.DivisionName) && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                           select a.ACAndBWPropRENxtFin).Sum();

                                                    row4[Regions.DivisionName] = @LedgerUnitSumSubGroupReport != 0.00 ? @LedgerUnitSumSubGroupReport.ToString("F4") : "-";

                                                }
                                                else if (ReportName != null && ReportName.Equals("Consolidated Actual Half Year"))
                                                {
                                                    LedgerUnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                           join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                           where (b.DivisionType.Equals(Regions.DivisionName) && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                           select a.ActCurrFinTill2ndQuart).Sum();

                                                    row4[Regions.DivisionName] = @LedgerUnitSumSubGroupReport != 0.00 ? @LedgerUnitSumSubGroupReport.ToString("F4") : "-";

                                                }
                                                else if (ReportName != null && ReportName.Equals("Consolidated Revised Estimates"))
                                                {
                                                    LedgerUnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                           join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                           where (b.DivisionType.Equals(Regions.DivisionName) && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                           select a.ACAndBWPropRECurrFin).Sum();

                                                    row4[Regions.DivisionName] = @LedgerUnitSumSubGroupReport != 0.00 ? @LedgerUnitSumSubGroupReport.ToString("F4") : "-";

                                                }
                                                else if (ReportName != null && ReportName.Equals("Consolidated Budget Estimates " + @NextAcademicYear))
                                                {
                                                    LedgerUnitSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                                           join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                                           where (b.DivisionType.Equals(Regions.DivisionName) && a.LedgerNumber.Equals(@Ledgers.LedgerNo) && a.SubGroupNumber.Equals(@Subs.SubGroupNo))
                                                                                           select a.ACAndBWPropRENxtFin).Sum();

                                                    row4[Regions.DivisionName] = @LedgerUnitSumSubGroupReport != 0.00 ? @LedgerUnitSumSubGroupReport.ToString("F4") : "-";

                                                }
                                            }
                                            dataTable2.Rows.Add(row4);
                                        }

                                    }

                                }

                                DataRow row5 = dataTable2.NewRow();

                                row5["HEAD NO."] = " ";
                                row5["SUB HEAD NO."] = "Total";
                                row5["HEAD NAME"] = "Total";
                                row5[@Report] = @SumGroupReport != 0.00 ? @SumGroupReport.ToString("F4") : "-";
                                row5["HEADQUARTERS"] = @HQSumGroupReport != 0.00 ? @HQSumGroupReport.ToString("F4") : "-";
                                row5["ALL REGIONAL OFFICES"] = @ROSumGroupReport != 0.00 ? @ROSumGroupReport.ToString("F4") : "-";
                                row5["ALL COEs"] = @COESumGroupReport != 0.00 ? @COESumGroupReport.ToString("F4") : "-";
                                foreach(var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals("Unit")))
                                {

                                    if (ReportName != null && ReportName.Equals("Consolidated Actual"))
                                    {
                                        UnitSumGroupReport = (Double)(from a in mymodel.Detailss
                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                      where (b.DivisionName.Equals(Regions.DivisionName))
                                                                      select a.ActPrevFin).Sum();
                                        row5[Regions.DivisionName] = @COESumGroupReport != 0.00 ? @UnitSumGroupReport.ToString("F4") : "-";

                                    }
                                    else if (ReportName != null && ReportName.Equals("Consolidated Actual Half Year"))
                                    {
                                        //SumGroupReport = (Double)(from a in mymodel.Detailss where (a.GroupNumber.Equals(@item.GroupNo)) select a.ActCurrFinTill2ndQuart).Sum();
                                        UnitSumGroupReport = (Double)(from a in mymodel.Detailss
                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                      where (b.DivisionType.Equals("Centre of Excellence"))
                                                                      select a.ActCurrFinTill2ndQuart).Sum();
                                        row5[Regions.DivisionName] = @COESumGroupReport != 0.00 ? @UnitSumGroupReport.ToString("F4") : "-";

                                    }
                                    else if (ReportName != null && ReportName.Equals("Consolidated Revised Estimates"))
                                    {
                                        UnitSumGroupReport = (Double)(from a in mymodel.Detailss
                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                      where (b.DivisionType.Equals("Centre of Excellence"))
                                                                      select a.ACAndBWPropRECurrFin).Sum();
                                        row5[Regions.DivisionName] = @COESumGroupReport != 0.00 ? @UnitSumGroupReport.ToString("F4") : "-";

                                    }
                                    else if (ReportName != null && ReportName.Equals("Consolidated Budget Estimates " + @NextAcademicYear));
                                    {
                                        UnitSumGroupReport = (Double)(from a in mymodel.Detailss
                                                                      join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                      where (b.DivisionType.Equals("Centre of Excellence"))
                                                                      select a.ACAndBWPropRENxtFin).Sum();
                                        row5[Regions.DivisionName] = @COESumGroupReport != 0.00 ? @UnitSumGroupReport.ToString("F4") : "-";

                                    }
                                }
                                dataTable2.Rows.Add(row5);
                            }

                            else
                            {
                                dataTable1.Rows.Add(item.GroupNo + " " + item.GroupName);
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

                                foreach (var Subs in mymodel.SubGroupss.Where(d => d.GroupNo.Equals(item.GroupNo)))
                                {
                                    DataRow row = dataTable1.NewRow();
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
                                    row[@Report] = SumSubGroupReport != 0.00 ? SumSubGroupReport.ToString("F4") : "-";

                                    foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                                    {
                                        var result = (from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(@Subs.SubGroupNo))) select a).FirstOrDefault();

                                        if (ReportName != null && ReportName.Equals("Actual"))
                                        {

                                            if (Subs.RequireInput)
                                            {
                                                var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(@Subs.SubGroupNo))) select a.ActPrevFin).Sum();
                                                row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F4") : "-";
                                            }
                                            else
                                            {
                                                row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActPrevFin).ToString("F4") : "-";
                                            }
                                        }
                                        else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                                        {
                                            if (Subs.RequireInput)
                                            {
                                                var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(Subs.SubGroupNo))) select a.ActCurrFinTill2ndQuart).Sum();
                                                row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F4") : "-";
                                            }
                                            else
                                            {
                                                row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActCurrFinTill2ndQuart).ToString("F4") : "-";
                                            }
                                        }
                                        else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                                        {
                                            if (Subs.RequireInput)
                                            {
                                                var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(@Subs.SubGroupNo))) select a.ACAndBWPropRECurrFin).Sum();
                                                row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F4") : "-";
                                            }
                                            else
                                            {
                                                row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRECurrFin).ToString("F4") : "-";
                                            }
                                        }
                                        else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                                        {
                                            if (Subs.RequireInput)
                                            {
                                                var LedgersSum = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.SubGroupNumber.Equals(Subs.SubGroupNo))) select a.ACAndBWPropRENxtFin).Sum();
                                                row[Regions.DivisionName] = LedgersSum != 0.00 ? Convert.ToDecimal(LedgersSum).ToString("F4") : "-";
                                            }
                                            else
                                            {
                                                row[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRENxtFin).ToString("F4") : "-";
                                            }
                                        }

                                    }

                                    dataTable1.Rows.Add(row);

                                    if (LedgerCount > 0)
                                    {
                                        foreach (var Ledgers in mymodel.Ledgerss.Where(d => d.SubGroupNo.Equals(Subs.SubGroupNo)))
                                        {
                                            DataRow row2 = dataTable1.NewRow();
                                            var BudEstCurrFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.BudEstCurrFin).Sum();
                                            var ActPrevFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ActPrevFin).Sum();
                                            var ActCurrFinTill2ndQuartLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ActCurrFinTill2ndQuart).Sum();
                                            var RevEstCurrFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ACAndBWPropRECurrFin).Sum();
                                            var BudgEstNexFinLedgerSum = (from a in mymodel.Detailss where (a.SubGroupNumber.Equals(@Subs.SubGroupNo) && a.LedgerNumber.Equals(@Ledgers.LedgerNo)) select a.ACAndBWPropRENxtFin).Sum();

                                            var LedgerSumSubGroupReport = 0.00;

                                            if (ReportName != null && ReportName.Equals("Actual"))
                                            {
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.LedgerNumber.Equals(Ledgers.LedgerNo))
                                                                             select a.ActPrevFin).Sum();

                                            }
                                            else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                                            {
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.LedgerNumber.Equals(Ledgers.LedgerNo))
                                                                             select a.ActCurrFinTill2ndQuart).Sum();
                                            }
                                            else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                                            {
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.LedgerNumber.Equals(Ledgers.LedgerNo))
                                                                             select a.ACAndBWPropRECurrFin).Sum();
                                            }
                                            else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                                            {
                                                LedgerSumSubGroupReport = (Double)(from a in mymodel.Detailss
                                                                             join b in mymodel.Divisionss on a.DivisionID equals b.DivisionID
                                                                             where (b.DivisionType.Equals(SelectedDivisionTypeName) && a.LedgerNumber.Equals(Ledgers.LedgerNo))
                                                                             select a.ACAndBWPropRENxtFin).Sum();
                                            }

                                            row2["HEAD NO."] = " ";
                                            row2["SUB HEAD NO."] = Ledgers.LedgerNo;
                                            row2["HEAD NAME"] = Ledgers.LedgerName;
                                            row2[@Report] = @LedgerSumSubGroupReport != 0.00 ? @LedgerSumSubGroupReport.ToString("F") : "-";

                                            foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                                            {
                                                var result = (from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (a.LedgerNumber.Equals(Ledgers.LedgerNo))) select a).FirstOrDefault();

                                                if (ReportName != null && ReportName.Equals("Actual"))
                                                {
                                                    row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActPrevFin).ToString("F4") : "-";
                                                }
                                                else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                                                {
                                                    row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ActCurrFinTill2ndQuart).ToString("F4") : "-";
                                                }
                                                else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                                                {
                                                    row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRECurrFin).ToString("F4") : "-";
                                                }
                                                else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                                                {
                                                    row2[Regions.DivisionName] = result != null ? Convert.ToDecimal(result.ACAndBWPropRENxtFin).ToString("F4") : "-";
                                                }
                                            }
                                            dataTable1.Rows.Add(row2);

                                        }
                                    }

                                }
                                DataRow row1 = dataTable1.NewRow();

                                row1["HEAD NO."] = " ";
                                row1["SUB HEAD NO."] = "Total";
                                row1["HEAD NAME"] = "Total";
                                row1[@Report] = SumGroupReport != 0.00 ? SumGroupReport.ToString("F4") : "-";

                                foreach (var Regions in mymodel.Divisionss.Where(d => d.DivisionType.Equals(SelectedDivisionTypeName)))
                                {
                                    if (ReportName != null && ReportName.Equals("Actual"))
                                    {
                                        var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ActPrevFin).Sum();
                                        row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F4") : "-";
                                    }
                                    else if (ReportName != null && ReportName.Equals("Actual Half Year"))
                                    {
                                        var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ActCurrFinTill2ndQuart).Sum();
                                        row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F4") : "-";
                                    }
                                    else if (ReportName != null && ReportName.Equals("Revised Estimates"))
                                    {
                                        var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ACAndBWPropRECurrFin).Sum();
                                        row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F4") : "-";
                                    }
                                    else if (ReportName != null && ReportName.Equals("Budget Estimates " + @NextAcademicYear))
                                    {
                                        var result = (Double)(from a in mymodel.Detailss where ((a.DivisionID == Regions.DivisionID) && (item.GroupNo.Equals(a.GroupNumber))) select a.ACAndBWPropRENxtFin).Sum();
                                        row1[Regions.DivisionName] = result != 0.00 ? Convert.ToDecimal(result).ToString("F4") : "-";
                                    }
                                }

                                dataTable1.Rows.Add(row1);

                               
                            }
                        
                        }

                        
                    }
                    else
                    {
                        return NotFound();
                    }

                }

                if (ReportName != null && ReportName.Equals("Headwise Consolidation"))
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add();
                        var Row1 = ws.Range(ws.Cell(1, 1), ws.Cell(1, dataTable.Columns.Count));
                        var Row2 = ws.Range(ws.Cell(2, 1), ws.Cell(2, dataTable.Columns.Count));
                        var Row3a = ws.Range(ws.Cell(3, 1), ws.Cell(3,4));
                        var Row3b = ws.Range(ws.Cell(3,5), ws.Cell(3,dataTable.Columns.Count));
                        var Col2 = ws.Column(2);
                        var Col3 = ws.Column(3);

                        Row1.Merge();
                        Row1.Value = "CENTRAL BOARD OF SECONDARY EDUCATION";
                        Row1.Style.Font.Bold = true;
                        Row1.Style.Fill.BackgroundColor = XLColor.BabyPink;

                        Row2.Merge();
                        Row2.Value = "Shiksha Kendra 2, Community Centre, Preet Vihar, Delhi - 11 092";
                        Row2.Style.Font.Bold = true;
                        Row2.Style.Fill.BackgroundColor = XLColor.Champagne;

                        Row3a.Merge();
                        Row3a.Value = Rep + " in " + AcademicYear + " for " + DivisionType;
                        Row3a.Style.Font.Bold = true;
                        Row3a.Style.Fill.BackgroundColor = XLColor.LightGoldenrodYellow;
                        Row3a.Style.Font.SetFontColor(XLColor.Green);

                        Row3b.Merge();
                        Row3b.Value = "* FIGURES IN CRORES";
                        Row3b.Style.Font.Bold = true;
                        Row3b.Style.Fill.BackgroundColor = XLColor.LightGoldenrodYellow;
                        Row3b.Style.Font.SetFontColor(XLColor.Red);

                        wb.Worksheet(1).Cell(4, 1).InsertTable(dataTable);

                        var Rows = Col2.Search("Total") ;
                        
                            foreach (var row in Rows)
                            {
                                var CellAddress = row.Address;
                                var RowNo = CellAddress.RowNumber;
                                var Range = ws.Range(ws.Cell(CellAddress.RowNumber, 2), ws.Cell(CellAddress.RowNumber, dataTable.Columns.Count));
                                Range.Style.Fill.BackgroundColor = XLColor.YellowGreen;
                            }

                        ws.RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.RightBorder = XLBorderStyleValues.Thin;

                        int lastrow = ws.LastRowUsed().RowNumber();
                        var rows = ws.Rows(4, lastrow);
                        foreach (IXLRow row in rows)
                        {
                            if (!ws.Cell(row.RowNumber(), 1).IsEmpty() && ws.Cell(row.RowNumber(), 2).IsEmpty() && ws.Cell(row.RowNumber(), 3).IsEmpty())
                            {

                                var Range = ws.Range(ws.Cell(row.RowNumber(), 1), ws.Cell(row.RowNumber(), 4));
                                var Content = ws.Cell(row.RowNumber(), 1).GetText();
                                Range.Merge();
                                Range.Style.Fill.BackgroundColor = XLColor.CanaryYellow;
                                Range.Style.Font.SetBold(true);
                            }
                        }

                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }

                }
                else if (ReportName != null && ReportName.Contains("Consolidated"))
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add();
                        
                        var Row1 = ws.Range(ws.Cell(1, 1), ws.Cell(1, dataTable2.Columns.Count));
                        var Row2 = ws.Range(ws.Cell(2, 1), ws.Cell(2, dataTable2.Columns.Count));
                        var Row3a = ws.Range(ws.Cell(3, 1), ws.Cell(3, 4));
                        var Row3b = ws.Range(ws.Cell(3, 5), ws.Cell(3, dataTable2.Columns.Count));
                        var Col2 = ws.Column(2);
                        var Col3 = ws.Column(3);

                        Row1.Merge();
                        Row1.Value = "CENTRAL BOARD OF SECONDARY EDUCATION";
                        Row1.Style.Font.Bold = true;
                        Row1.Style.Fill.BackgroundColor = XLColor.BabyPink;
                        Row1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        Row2.Merge();
                        Row2.Value = "Shiksha Kendra 2, Community Centre, Preet Vihar, Delhi - 11 092";
                        Row2.Style.Font.Bold = true;
                        Row2.Style.Fill.BackgroundColor = XLColor.Champagne;
                        Row2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        Row3a.Merge();
                        Row3a.Value = Rep + " in " + AcademicYear;
                        Row3a.Style.Font.Bold = true;
                        Row3a.Style.Fill.BackgroundColor = XLColor.LightGoldenrodYellow;
                        Row3a.Style.Font.SetFontColor(XLColor.Green);
                        Row3a.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        Row3b.Merge();
                        Row3b.Value = "* FIGURES IN CRORES";
                        Row3b.Style.Font.Bold = true;
                        Row3b.Style.Fill.BackgroundColor = XLColor.LightGoldenrodYellow;
                        Row3b.Style.Font.SetFontColor(XLColor.Red);
                        Row3b.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        wb.Worksheet(1).Cell(4, 1).InsertTable(dataTable2);

                        var Rows = Col2.Search("Total");

                        foreach (var row in Rows)
                        {
                            var CellAddress = row.Address;
                            var RowNo = CellAddress.RowNumber;
                            var Range = ws.Range(ws.Cell(CellAddress.RowNumber, 2), ws.Cell(CellAddress.RowNumber, dataTable2.Columns.Count));
                            Range.Style.Fill.BackgroundColor = XLColor.YellowGreen;
                        }

                        ws.RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.RightBorder = XLBorderStyleValues.Thin;

                        int lastrow = ws.LastRowUsed().RowNumber();
                        var rows = ws.Rows(4, lastrow);
                        foreach (IXLRow row in rows)
                        {
                            if (!ws.Cell(row.RowNumber(), 1).IsEmpty() && ws.Cell(row.RowNumber(), 2).IsEmpty() && ws.Cell(row.RowNumber(), 3).IsEmpty())
                            {

                                var Range = ws.Range(ws.Cell(row.RowNumber(), 1), ws.Cell(row.RowNumber(), 4));
                                var Content = ws.Cell(row.RowNumber(), 1).GetText();
                                Range.Merge();
                                Range.Style.Fill.BackgroundColor = XLColor.CanaryYellow;
                                Range.Style.Font.SetBold(true);
                            }
                        }

                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }
                }
                else
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add();
                        ws.Columns(1, dataTable1.Columns.Count).AdjustToContents();
                        var Row1 = ws.Range(ws.Cell(1, 1), ws.Cell(1, dataTable1.Columns.Count));
                        var Row2 = ws.Range(ws.Cell(2, 1), ws.Cell(2, dataTable1.Columns.Count));
                        var Row3a = ws.Range(ws.Cell(3, 1), ws.Cell(3, 4));
                        var Row3b = ws.Range(ws.Cell(3, 5), ws.Cell(3, dataTable1.Columns.Count));
                        var Col2 = ws.Column(2);

                        Row1.Merge();
                        Row1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        Row1.Value = "CENTRAL BOARD OF SECONDARY EDUCATION";
                        Row1.Style.Font.Bold = true;
                        Row1.Style.Fill.BackgroundColor = XLColor.BabyPink;
                        Row1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        Row2.Merge();
                        Row2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        Row2.Value = "Shiksha Kendra 2, Community Centre, Preet Vihar, Delhi - 11 092";
                        Row2.Style.Font.Bold = true;
                        Row2.Style.Fill.BackgroundColor = XLColor.Champagne;
                        Row2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        Row3a.Merge();
                        Row3a.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        Row3a.Value = "Consolidated " + Rep + " in " + AcademicYear + " for " + DivisionType;
                        Row3a.Style.Font.Bold = true;
                        Row3a.Style.Fill.BackgroundColor = XLColor.LightGoldenrodYellow;
                        Row3a.Style.Font.SetFontColor(XLColor.Green);
                        Row3a.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        Row3b.Merge();
                        Row3b.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        Row3b.Value = "* FIGURES IN CRORES";
                        Row3b.Style.Font.Bold = true;
                        Row3b.Style.Fill.BackgroundColor = XLColor.LightGoldenrodYellow;
                        Row3b.Style.Font.SetFontColor(XLColor.Red);

                        wb.Worksheet(1).Cell(4, 1).InsertTable(dataTable1);

                        var Rows = Col2.Search("Total");
                        foreach (var row in Rows)
                        {
                            var CellAddress = row.Address;
                            var RowNo = CellAddress.RowNumber;
                            var Range = ws.Range(ws.Cell(CellAddress.RowNumber, 2), ws.Cell(CellAddress.RowNumber, dataTable1.Columns.Count));
                            Range.Style.Fill.BackgroundColor = XLColor.YellowGreen;
                            Range.Style.Font.SetBold(true);
                        }

                        ws.RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        ws.RangeUsed().Style.Border.RightBorder = XLBorderStyleValues.Thin;

                        int lastrow = ws.LastRowUsed().RowNumber();
                        var rows = ws.Rows(4, lastrow);
                        foreach (IXLRow row in rows)
                        {
                            if (!ws.Cell(row.RowNumber(), 1).IsEmpty() && ws.Cell(row.RowNumber(), 2).IsEmpty() && ws.Cell(row.RowNumber(), 3).IsEmpty())
                            {

                                var Range = ws.Range(ws.Cell(row.RowNumber(), 1), ws.Cell(row.RowNumber(), 4));
                                var Content = ws.Cell(row.RowNumber(), 1).GetText();
                                Range.Merge();
                                Range.Style.Fill.BackgroundColor = XLColor.CanaryYellow;
                                Range.Style.Font.SetBold(true);
                            }
                        }


                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }
                }
            }
            else
            {
                return NotFound();
            }

               return Ok();
            }

    }
}
