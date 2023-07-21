using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class SectionDetails
    {
        public int id { get; set; }

        [Key]
        [Required]
        public int SectionNo { get; set; }

        [StringLength(200)]
        public String? SectionName { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public ICollection<GroupDetails>? Groups { get; set; }

    }
}