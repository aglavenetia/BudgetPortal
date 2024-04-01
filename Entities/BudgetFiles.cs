using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace BudgetPortal.Entities
{
    public class BudgetFiles
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DivisionID { get; set; }

        [Required]
        public int FinancialYear1 { get; set; }

        [Required]
        public int FinancialYear2 { get; set; }

        [StringLength(10)]
        [Required]
        public String? SubGroupNumber { get; set; }

        [StringLength(10)]
        [Required]
        public String? GroupNumber { get; set; }

        [StringLength(15)]
        public String? LedgerNumber { get; set; }

        [Required]
        public int SectionNumber { get; set; }

        public String? FileName { get; set; }
        public String? SupportingDocumentPath { get; set; }
        public DateTime UploadedDateTime { get; set; }

    }
}
