using BudgetPortal.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetPortal.ViewModel
{
    public class InterimRevision
    {
        public IEnumerable<BudgetSections>? Sectionss { get; set; }
        public IEnumerable<BudgetGroups>? Groupss { get; set; }

        public IEnumerable<BudgetLedgers>? SubGroupss { get; set; }

        public IEnumerable<BudgetSubLedgers>? Ledgerss { get; set; }

        public IEnumerable<BudgetDetails>? Detailss { get; set; }

        public IEnumerable<BudgetdetailsStatus>? Statuss { get; set; }
        
        public IEnumerable<BudgetDetailsApproved>? Approved { get; set; }

        public IEnumerable<Division>? Divisionss { get; set; }

        public IEnumerable<AcademicYears>? AcademicYearss { get; set; }

        public int BudgetApprovedStatus { get; set; }

        public String SectionName { get; set; }
        public String GroupName { get; set; }
        public String SubGroupLedgerName { get; set; }
        public String? SelectedDivisionName { get; set; }
        public String? SelectedAcademicYear { get; set; }
        public List<String> SubGroupNameOrLedgerName { get; set; }
        public Boolean IsEnabled { get; set; }
        public String EditEnabled { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> BudEstCurrFin { get; set; }
        
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> InterimRev { get; set; }
        
        [Column(TypeName = "money")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        public List<Decimal> ProvisionalRE { get; set; }

        public IEnumerable<SelectListItem>? DivisionNames { get; set; }

        [Display(Name = "Division Names")]
        [Required(ErrorMessage = "Division Name Required")]
        public String? SelectedDivisionID { get; set; }

        public IEnumerable<SelectListItem>? AcademicYears { get; set; }

        [Display(Name = "Financial Year")]
        [Required(ErrorMessage = "Academic Year Required")]
        public String? SelectedAcademicYearID { get; set; }
    }
}
