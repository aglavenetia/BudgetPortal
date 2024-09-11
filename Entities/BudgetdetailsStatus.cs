using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace BudgetPortal.Entities
{
    [PrimaryKey(nameof(DivisionID), nameof(FinancialYear1), nameof(FinancialYear2), nameof(SectionNumber), nameof(GroupNumber))]
    public class BudgetdetailsStatus
    {
        public int DivisionID { get; set; }

        [Required]
        public int FinancialYear1 { get; set; }

        [Required]
        public int FinancialYear2 { get; set; }

        [Required]
        public int SectionNumber { get; set; }

        [StringLength(10)]
        [Required]
        public String? GroupNumber { get; set; }

        public Boolean DelegateEditStatus { get; set; }

        public Boolean AdminEditStatus { get; set; }

        public Boolean ACBWSubmission { get; set; }

        public Boolean ChairpersonApproval { get; set; }

        public Boolean FinCommitteeApproval { get; set; }

        public Boolean GenBodyApproval { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public String? Remarks { get; set; }

        public String? AdditionalComments { get; set; }

        public Boolean? IsHeadApproved   { get; set; }

        public String? eoffFileNo { get; set; }
    }
}
