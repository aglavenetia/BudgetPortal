using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

using Microsoft.EntityFrameworkCore;

namespace BudgetPortal.Entities
{
    [PrimaryKey(nameof(DivisionID), nameof(FinancialYear1), nameof(FinancialYear2), nameof(SectionNumber), nameof(GroupNumber), nameof(SubGroupNumber), nameof(LedgerNumber))]
    public class BudgetDetailsApproved
    {
        public int DivisionID { get; set; }

        [Required]
        public int FinancialYear1 { get; set; }

        [Required]
        public int FinancialYear2 { get; set; }

        [Required]
        public int SectionNumber { get; set; }

        [StringLength(10)]
        [Required]
        public String? GroupNumber { get; set; }

        [StringLength(15)]
        [Required]
        public String SubGroupNumber { get; set; }

        [StringLength(15)]
        public String? LedgerNumber { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal BudEstCurrFinACandBW { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal RevEstCurrFinACandBW { get; set; }

        [Column(TypeName = "money")]
        [Required]
        public Decimal BudEstNextFin { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
