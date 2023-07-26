using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class BudgetSubGroups
    {
        [Key]
        [StringLength(10)]
        public String SubGroupNo { get; set; }

        [StringLength(200)]
        public String subGroupName { get; set; }

        public String GroupNo { get; set; }

        public BudgetGroups? groups { get; set; }

        public ICollection<BudgetLedgers>? Ledgers { get; set; }

        public DateTime CreatedDateTime { get; set; }

        // ICollection<BudgetDetails> Budgets { get; set; }
    }
}
