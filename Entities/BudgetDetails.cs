using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace BudgetPortal.Entities
{
    public class BudgetDetails
    {
        public int Id { get; set; }

        public int DivisionID { get; set; }

        [Required]
        public int FinancialYear1 { get; set; }

        [Required]
        public int FinancialYear2 { get; set; }

        [StringLength(10)]
        public String SubGroupNumber { get; set; }

        [StringLength(10)]
        public String GroupNumber { get; set; }

        [StringLength(10)]
        public String LedgerNumber { get; set; }
        public int SectionNumber { get; set; }

        [Column(TypeName = "money")]
        public Decimal BudEstCurrFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ActPrevFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ActCurrFinTill2ndQuart { get; set; }

        [Column(TypeName = "money")]
        public Decimal RevEstCurrFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public Decimal PerVarRevEstOverBudgEstCurrFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ACAndBWPropRECurrFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal BudgEstNexFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public Decimal PerVarRevEstOverBudgEstNxtFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ACAndBWPropRENxtFin { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public string Justification { get; set; }

        public string? SupportingDocumentPath { get; set; }

    }
}
