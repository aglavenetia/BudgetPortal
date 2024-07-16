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
    public class MultipleData
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


        public String EditEnabled { get; set; }

        public int BudgetApprovedStatus { get; set; }

        public Boolean? IsEnabled { get; set; }

        public List<String> SubGroupNameOrLedgerName { get; set; }

        public List<Boolean> HasDelegateSaved { get;set;}

        public List<Boolean> HasAdminSaved { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> BudEstCurrFin { get; set; }
        //public Decimal BudEstCurrFin { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> InterimRevEst { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ProvisionalRevEst { get; set; }

        [Required(ErrorMessage = "Actual for the Previous Financial Year Required")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ActPrevFin { get; set; }
        //public Decimal ActPrevFin { get; set; }

        [Required(ErrorMessage = "Actual for the Current Financial Year Till 2nd Quarter Required")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ActCurrFinTillsecondQuart { get; set; }
        //public Decimal ActCurrFinTill2ndQuart { get; set; }

        [Required(ErrorMessage = "Revised Estimates for the Current Financial Year Required")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> RevEstCurrFin { get; set; }
        //public Decimal RevEstCurrFin { get; set; }

        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}%")]
        public List<Decimal> PerVarRevEstOverBudgEstCurrFin { get; set; }
        //public Decimal PerVarRevEstOverBudgEstCurrFin { get; set; }

        [Required(ErrorMessage = "Please enter the values")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ACAndBWPropRECurrFin { get; set; }
        //public Decimal ACAndBWPropRECurrFin { get; set; }
        public List<String>? DelegateJustificationRevEst { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}%")]
        public List<Decimal> PerVarACBWRevEstOverBudgEstCurrFin { get; set; }
        //public Decimal PerVarRevEstOverBudgEstCurrFin { get; set; }

        public List<String>? ACBWJustificationRevEst { get; set; }

        [Required(ErrorMessage = "Budget Estimates for the Next Financial Year Required")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> BudgEstNexFin { get; set; }
        //public Decimal BudgEstNexFin { get; set; }

        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}%")]
        public List<Decimal> PerVarRevEstOverBudgEstNxtFin { get; set; }
        //public Decimal PerVarRevEstOverBudgEstNxtFin { get; set; }

        [Required(ErrorMessage = "Please enter the values")]
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ACAndBWPropRENxtFin { get; set; }
        //public Decimal ACAndBWPropRENxtFin { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}%")]
        public List<Decimal> PerVarACBWRevEstOverBudgEstNxtFin { get; set; }
        public List<String>? Justification { get; set; }
        //public String Justification { get; set; }

        public List<String>? ACBWJustificationBudgEstNxtFin { get; set; }

        public IEnumerable<SelectListItem>? DivisionNames { get; set; }

        [Display(Name = "Division Names")]
        [Required(ErrorMessage = "Division Names Required")]
        public String? SelectedDivisionID { get; set; }

        
        public IEnumerable<SelectListItem>? AcademicYears { get; set; }

        [Display(Name = "Financial Year")]
        [Required(ErrorMessage = "Academic Year Required")]
        public String? SelectedAcademicYearID { get; set; }
        public string? FileName { get; set; }
        public IFormFile? File { get; set; }
        public string? WaitingForFinalSubmission { get; set; }
        public string? WaitingForApprovalMessage { get; set; }
        public string? ApprovedMessage { get; set; }

    }
}
