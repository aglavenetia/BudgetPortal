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

namespace BudgetPortal.ViewModel
{
    public class MultipleData
    {
        public IEnumerable<BudgetSections> Sectionss { get; set; }
        public IEnumerable<BudgetGroups> Groupss { get; set; }

        public IEnumerable<BudgetSubGroups> SubGroupss { get; set; }

        public IEnumerable<BudgetLedgers> Ledgerss { get; set; }

        public IEnumerable<BudgetDetails> Detailss { get; set; }

        public String SectionName { get; set; }

        public String GroupName { get; set; }

        [Column(TypeName = "money")]
        public List<Decimal> BudEstCurrFin { get; set; }

        [Column(TypeName = "money")]
        public List<Decimal> ActPrevFin { get; set; }

        [Column(TypeName = "money")]
        public List<Decimal> ActCurrFinTill2ndQuart { get; set; }

        [Column(TypeName = "money")]
        public List<Decimal> RevEstCurrFin { get; set; }

        
        [Column(TypeName = "decimal(4,2)")]
        public List<Decimal> PerVarRevEstOverBudgEstCurrFin { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public List<Decimal> ACAndBWPropRECurrFin { get; set; }

        [Column(TypeName = "money")]
        public List<Decimal> BudgEstNexFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public List<Decimal> PerVarRevEstOverBudgEstNxtFin { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public List<Decimal> ACAndBWPropRENxtFin { get; set; }

        public List<String> Justification { get; set; }

        public IEnumerable<BudgetReports> Reports { get; set; }
        public IEnumerable<SelectListItem> ReportNames { get; set; }
        public String SelectedReportID { get; set; }
        public String SelectedReportName { get; set; }

        [Required(ErrorMessage = "Please select a Division Name")]
        public IEnumerable<Division> Divisionss { get; set; }
        public IEnumerable<SelectListItem> DivisionNames { get; set; }
        public String SelectedDivisionID { get; set; }
        public String SelectedDivisionName { get; set; }

        public List<SelectListItem> DivisionTypeNames { get; set; }
        public String SelectedDivisionTypeID { get; set; }
        public String SelectedDivisionTypeName { get; set; }

        [Required(ErrorMessage = "Please select the Academic Year")]
        public IEnumerable<AcademicYears> AcademicYearss { get; set; }
        public IEnumerable<SelectListItem> AcademicYears { get; set; }
        public String SelectedAcademicYearID { get; set; }
        public String SelectedAcademicYear { get; set; }
        
    }
}
