using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace BudgetPortal.Entities
{
    [PrimaryKey(nameof(DivisionID), nameof(FinancialYear1), nameof(FinancialYear2), nameof(SectionNumber), nameof(GroupNumber), nameof(SubGroupNumber), nameof(LedgerNumber))]
    public class BudgetDetails
    {
        public int DivisionID { get; set; }

        [Required]
        public int FinancialYear1 { get; set; }

        [Required]
        public int FinancialYear2 { get; set; }

        [StringLength(15)]
        [Required]
        public String? SubGroupNumber { get; set; }

        [StringLength(10)]
        [Required]
        public String? GroupNumber { get; set; }

        [StringLength(15)]
        public String? LedgerNumber { get; set; }
        
        [Required]
        public int SectionNumber { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal BudEstCurrFin { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal ActPrevFin { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal ActCurrFinTill2ndQuart { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal RevEstCurrFin { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public Decimal PerVarRevEstOverBudgEstCurrFin { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public Decimal PerVarACBWRevEstOverBudgEstCurrFin { get; set; }

        public string? DelegateJustificationRevEst { get; set; }

        public string? ACBWJustificationRevEst { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal ACAndBWPropRECurrFin { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal InterimRevEst{ get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal ProvisionalRevEst { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal BudgEstNexFinProposed { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal BudgEstNexFin { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public Decimal PerVarRevEstOverBudgEstNxtFin { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public Decimal PerVarACBWRevEstOverBudgEstNxtFin { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal ACAndBWPropRENxtFin { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string? Justification { get; set; }

        public string? ACBWJustificationBudgEstNxtFin { get; set; }
        public string? SupportingDocumentPath { get; set; }
        public Boolean HasDelegateSaved { get; set; }
        public Boolean HasAdminSaved { get; set; }
    }
}
