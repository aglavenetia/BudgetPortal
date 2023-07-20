using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class Divisions
    {
        public int id { get; set; }

        [Key]
        [Required]
        public int DivisionID { get; set; }

        [StringLength(200)]
        public String DivisionName { get; set; }

        [StringLength(200)]
        public String DivisionType { get; set; }

        [StringLength(200)]
        public String DealingHandID { get; set; }

        public ICollection<BudgetDetails> Budgets { get; set; }
    }
}
