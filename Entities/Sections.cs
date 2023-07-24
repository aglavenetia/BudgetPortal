using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class Sections
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SectionNo { get; set; }

        [StringLength(200)]
        public String SectionName { get; set; }

        public DateTime CreatedDateTime { get; set; }
        //public ICollection<Groups> Groups { get; set; }

        
    }
}
