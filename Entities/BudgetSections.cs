using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace BudgetPortal.Entities
{
    public class BudgetSections
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SectionNo { get; set; }

        [StringLength(200)]
        public String SectionName { get; set; }

        public DateTime CreatedDateTime { get; set; }


        public virtual ICollection<BudgetGroups>? Groups { get; } = new List<BudgetGroups>();
       
    }
}
