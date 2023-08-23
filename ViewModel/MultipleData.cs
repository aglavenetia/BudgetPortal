using BudgetPortal.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
//using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetPortal.ViewModel
{
    public class MultipleData
    {
        public IEnumerable<BudgetSections> Sectionss { get; set; }
        public IEnumerable<BudgetGroups> Groupss { get; set; }

        public IEnumerable<BudgetSubGroups> SubGroupss { get; set; }

        public IEnumerable<BudgetLedgers> Ledgerss { get; set; }

        public IEnumerable<BudgetDetails> Detailss { get; set; }
        public String SubGroupNumber { get; set; }

        [StringLength(10)]
        public String GroupNumber { get; set; }

        [StringLength(10)]
        public String LedgerNumber { get; set; }
        public int SectionNumber { get; set; }

        [Column(TypeName = "money")]
        public Decimal BudEstCurrFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ActPrevFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ActCurrFinTill2ndQuart { get; set; }

        [Column(TypeName = "money")]
        public Decimal RevEstCurrFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public Decimal PerVarRevEstOverBudgEstCurrFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ACAndBWPropRECurrFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal BudgEstNexFin { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public Decimal PerVarRevEstOverBudgEstNxtFin { get; set; }

        [Column(TypeName = "money")]
        public Decimal ACAndBWPropRENxtFin { get; set; }

        public string Justification { get; set; }
        public IEnumerable<Division> Divisionss { get; set; }

        public IEnumerable<SelectListItem> DivisionNames { get; set; }
        public int SelectedDivisionID { get; set; }
        public String SelectedDivisionName { get; set; }

        public IEnumerable<AcademicYears> AcademicYearss { get; set; }
        public IEnumerable<SelectListItem> AcademicYears { get; set; }
        public String SelectedAcademicYear { get; set; }
        public int SelectedAcademicYearID { get; set; }



    }
}
