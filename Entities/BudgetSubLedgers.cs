using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class BudgetSubLedgers
    {
        [Key]
        [StringLength(15)]
        [DisplayName("Sub-Ledger No")]
        public String? LedgerNo { get; set; }

        [StringLength(200)]
        [DisplayName("Sub-Ledger Name")]
        public String? LedgerName { get; set; }

        [StringLength(15)]
        [DisplayName("Ledger No")]
        public String? SubGroupNo { get; set; }

        public BudgetLedgers? subGroups { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
