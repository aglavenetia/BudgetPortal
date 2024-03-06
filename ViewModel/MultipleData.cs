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
using BudgetPortal.Models;

namespace BudgetPortal.ViewModel
{
    public class MultipleData : ReponseModel
    {
        public IEnumerable<BudgetSections>? Sectionss { get; set; }
        public IEnumerable<BudgetGroups>? Groupss { get; set; }

        public IEnumerable<BudgetSubGroups>? SubGroupss { get; set; }

        public IEnumerable<BudgetLedgers>? Ledgerss { get; set; }

        public IEnumerable<BudgetDetails>? Detailss { get; set; }

        public IEnumerable<BudgetdetailsStatus>? Statuss { get; set; }

        public IEnumerable<BudgetDetailsApproved>? Approved { get; set; }

        public IEnumerable<Division>? Divisionss { get; set; }

        public IEnumerable<AcademicYears>? AcademicYearss { get; set; }

        public IEnumerable<BudgetFiles>? Filess { get; set; }
        public String SectionName { get; set; }
        public String GroupName { get; set; }
        public String SubGroupLedgerName { get; set; }

        public String? SelectedDivisionName { get; set; }
        public String? SelectedAcademicYear { get; set; }

        public Boolean? AdminEditStatus { get; set; }
        public Boolean? DelegateEditStatus { get; set; }

        public int PreviousYearAdminCount { get; set; }

        public List<String> SubGroupNameOrLedgerName { get; set; }

        [Required(ErrorMessage = "Please enter BudgetEstimates for the Current Financial Year")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> BudEstCurrFin { get; set; }
        //public Decimal BudEstCurrFin { get; set; }

        [Required(ErrorMessage = "Please enter Actual for the Previous Financial Year")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ActPrevFin { get; set; }
        //public Decimal ActPrevFin { get; set; }

        [Required(ErrorMessage = "Please enter Actual for the Current Financial Year Till 2nd Quarter")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ActCurrFinTillsecondQuart { get; set; }
        //public Decimal ActCurrFinTill2ndQuart { get; set; }

        [Required(ErrorMessage = "Please enter Revised Estimates for the Current Financial Year")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> RevEstCurrFin { get; set; }
        //public Decimal RevEstCurrFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> PerVarRevEstOverBudgEstCurrFin { get; set; }
        //public Decimal PerVarRevEstOverBudgEstCurrFin { get; set; }

        [Required(ErrorMessage = "Please enter the values")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ACAndBWPropRECurrFin { get; set; }
        //public Decimal ACAndBWPropRECurrFin { get; set; }

        [Required(ErrorMessage = "Please enter Budget Estimates for the Next Financial Year")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> BudgEstNexFin { get; set; }
        //public Decimal BudgEstNexFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> PerVarRevEstOverBudgEstNxtFin { get; set; }
        //public Decimal PerVarRevEstOverBudgEstNxtFin { get; set; }

        [Required(ErrorMessage = "Please enter the values")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ACAndBWPropRENxtFin { get; set; }
        //public Decimal ACAndBWPropRENxtFin { get; set; }

        public List<String>? Justification { get; set; }
        //public String Justification { get; set; }


        public IEnumerable<SelectListItem>? DivisionNames { get; set; }

        [Required(ErrorMessage = "Please select a Division Name")]
        public String? SelectedDivisionID { get; set; }




        [Display(Name = "Financial Year")]
        public IEnumerable<SelectListItem>? AcademicYears { get; set; }

        [Required(ErrorMessage = "Please select the Academic Year")]
        public String? SelectedAcademicYearID { get; set; }


        [Required(ErrorMessage = "Please enter file name")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }


    }
}
