using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class BudgetLedgers
    {
        [Key]
        [StringLength(10)]
        public String LedgerNo { get; set; }

        [StringLength(200)]
        public String LedgerName { get; set; }

        [StringLength(10)]
        public String SubGroupNo { get; set; }
        public BudgetSubGroups? subGroups { get; set; }
        public Boolean RequireInput { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
