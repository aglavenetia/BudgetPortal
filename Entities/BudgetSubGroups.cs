using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class BudgetSubGroups
    {
        [Key]
        [StringLength(15)]
        public String SubGroupNo { get; set; }

        [StringLength(200)]
        [DisplayName("SubGroupName")]
        public String subGroupName { get; set; }

        public String GroupNo { get; set; }

        public BudgetGroups? groups { get; set; }

        public ICollection<BudgetLedgers>? Ledgers { get; set; }

        [DisplayName("Ledgers Require Input ?")]
        public Boolean RequireInput { get; set; }

        public DateTime CreatedDateTime { get; set; }

        // ICollection<BudgetDetails> Budgets { get; set; }
    }
}
