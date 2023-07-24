using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class Ledgers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LedgerNo { get; set; }

        [StringLength(200)]
        public String LedgerName { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public SubGroups subGroups { get; set; }
    }
}
