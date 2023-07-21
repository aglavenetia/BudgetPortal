using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class SubGroupDetails
    {
        public int id { get; set; }

        [Key]
        [Required]
        public int SubGroupNo { get; set; }

        [StringLength(200)]
        public String? subGroupName { get; set; }

        public GroupDetails? Groups { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public ICollection<LedgerDetails>? Ledgers { get; set; }

        public ICollection<BudgetDetails>? Budgets { get; set; }
    }
}
