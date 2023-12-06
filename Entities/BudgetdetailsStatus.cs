﻿using Microsoft.EntityFrameworkCore;
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
        public String GroupNumber { get; set; }

        public Boolean DelegateEditStatus { get; set; }

        public Boolean AdminEditStatus { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
