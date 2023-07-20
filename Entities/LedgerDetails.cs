using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class LedgerDetails
    {
        public int id { get; set; }

        [Key]
        [Required]
        public int LedgerNo { get; set; }

        [StringLength(200)]
        public String LedgerName { get; set; }
 
        public SubGroupDetails subGroups { get; set; }
    }
}
