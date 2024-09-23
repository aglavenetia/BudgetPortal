using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class BudgetGroups
    {
        [Key]
        [StringLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String? GroupNo { get; set; }

        [StringLength(200)]
        public String? GroupName { get; set; }

        public int SectionNo { get; set; }

        public BudgetSections? Sections { get; set; } = null!;

        public ICollection<BudgetLedgers>? SubGroups { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
