using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class BudgetLedgers
    {
        [Key]
        [StringLength(15)]
        [DisplayName("Ledger No")]
        public String SubGroupNo { get; set; }

        [StringLength(200)]
        [DisplayName("Ledger Name")]
        public String subGroupName { get; set; }

        public String GroupNo { get; set; }

        public BudgetGroups? groups { get; set; }

        public ICollection<BudgetSubLedgers>? Ledgers { get; set; }

        [DisplayName("Does Ledger has Sub-Ledgers ?")]
        public Boolean RequireInput { get; set; }

        public DateTime CreatedDateTime { get; set; }

        // ICollection<BudgetDetails> Budgets { get; set; }
    }
}
