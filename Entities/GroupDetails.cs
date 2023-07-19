using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetPortal.Entities
{
    public class GroupDetails
    {
        int Id { get; set; }

        [Key]
        [Required]
        public int GroupNo { get; set; }

        [StringLength(200)]
        public String GroupName { get; set; }

        public int SectionNumber { get; set; }

        [ForeignKey("SectionNo")]
        public ICollection<SectionDetails> GroupSectionNo { get; set; }
    }
}
