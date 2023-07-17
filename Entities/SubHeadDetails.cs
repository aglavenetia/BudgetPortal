using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class SubHeadDetails
    {
        public int id { get; set; }

        public int HeadNumber { get; set; }
        
        [Required]
        public int SubHeadNo { get; set; }

        [StringLength(200)]
        public String SubHeadName { get; set; }

        [ForeignKey("HeadNumber")]
        public ICollection<HeadDetails> HeadNo { get; set; }
    }
}
