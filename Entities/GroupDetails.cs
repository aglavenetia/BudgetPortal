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

        public SectionDetails sections { get; set; }

        public ICollection<SubGroupDetails> SubGroups { get; set; }

    }
}
