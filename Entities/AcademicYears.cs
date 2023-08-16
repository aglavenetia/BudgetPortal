using Microsoft.Build.Framework;

namespace BudgetPortal.Entities
{
    public class AcademicYears
    {
        public int Id { get; set; }

        [Required]
        public int Year1 { get; set; }
        [Required]
        public int Year2 { get; set; }
    }
}
