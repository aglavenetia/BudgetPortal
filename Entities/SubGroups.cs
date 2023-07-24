using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class SubGroups
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int SubGroupNo { get; set; }

        [StringLength(200)]
        public String subGroupName { get; set; }

        public Groups groups { get; set; }

        public DateTime CreatedDateTime { get; set; }

        //public ICollection<Ledgers> Ledgers { get; set; }

        // ICollection<BudgetDetails> Budgets { get; set; }
    }
}
