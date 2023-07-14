using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class HeadDetails
    {
        public int id { get; set; }

        [Required]
        public int HeadNo { get; set; }

        [StringLength(200)]
        public String HeadName { get; set; }

        [ForeignKey("SubSectionNo")]
        public ICollection<SubSectionDetails> SubSectionNo { get; set; }
    }
}
