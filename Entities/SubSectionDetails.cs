using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetPortal.Entities
{
    public class SubSectionDetails
    {
        public int id { get; set; }

        [Required]
        public int SubSectionNo { get; set; }

        [StringLength(200)]
        public String SubSectionName { get; set; }

        [ForeignKey("SectionNo")]
        public ICollection<SectionDetails> SectionNo { get; set; }
    }
}
