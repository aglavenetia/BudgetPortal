using BudgetPortal.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
//using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace BudgetPortal.ViewModel
{
    public class ReportView
    {
        public IEnumerable<BudgetSections>? Sectionss { get; set; }
        public IEnumerable<BudgetGroups>? Groupss { get; set; }

        public IEnumerable<BudgetLedgers>? SubGroupss { get; set; }

        public IEnumerable<BudgetSubLedgers>? Ledgerss { get; set; }
        public IEnumerable<BudgetDetailsApproved>? Approved { get; set; }
        public IEnumerable<BudgetDetails>? Detailss { get; set; }
        public IEnumerable<BudgetReports>? Reports { get; set; }
        public IEnumerable<Division>? Divisionss { get; set; }

        public List<SelectListItem>? DivisionTypeNames { get; set; }

        [Display(Name = "DivisionType Names")]
        [Required(ErrorMessage = "DivisionType Names are Required")]
        public String? SelectedDivisionTypeID { get; set; }
        public String? SelectedDivisionTypeName { get; set; }
        
        public IEnumerable<SelectListItem>? ReportNames { get; set; }

        [Display(Name = "Report Names")]
        [Required(ErrorMessage = "Please select any Report")]
        public String? SelectedReportID { get; set; }
        public String? SelectedReportName { get; set; }

        
        public IEnumerable<SelectListItem>? AcademicYears { get; set; }

        [Display(Name = "Financial Year")]
        [Required(ErrorMessage = "Academic Year Required")]
        public String? SelectedAcademicYearID { get; set; }
        
        public String? SelectedAcademicYear { get; set; }

        
        public String? SelectedDivisionName { get; set; }
        public String? SectionName { get; set; }

        public String? GroupName { get; set; }

        public Export ExportTypes { get; set; }
    }

    public enum Export
    {
        [Description("Export Report AS PDF")]
        ExportReportAsPDF,
        [Description("Export Report As Excel")]
        ExportReportAsExcel,
    }
}
