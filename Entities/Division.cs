using System.ComponentModel.DataAnnotations;

namespace BudgetPortal.Entities
{
    public class Division
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int DivisionID { get; set; }

        [StringLength(200)]
        public String DivisionName { get; set; }

        [StringLength(200)]
        public String DivisionType { get; set; }

        [StringLength(200)]
        public String DealingHandID { get; set; }

        [StringLength(200)]
        public String State { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
