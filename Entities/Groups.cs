using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class Groups
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupNo { get; set; }

        [StringLength(200)]
        public String GroupName { get; set; }

        public Sections sections { get; set; }

        public DateTime CreatedDateTime { get; set; }
        //public ICollection<SubGroups> SubGroups { get; set; }
    }
}
