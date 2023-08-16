using BudgetPortal.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
//using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetPortal.ViewModel
{
    public class MultipleData
    {
        public IEnumerable<BudgetSections> Sectionss { get; set; }
        public IEnumerable<BudgetGroups> Groupss { get; set; }

        public IEnumerable<BudgetSubGroups> SubGroupss { get; set; }

        public IEnumerable<BudgetLedgers> Ledgerss { get; set; }

        public IEnumerable<BudgetDetails> Detailss { get; set; }


        public IEnumerable<Division> Divisionss { get; set; }

        public IEnumerable<SelectListItem> DivisionNames { get; set; }

        public IEnumerable<AcademicYears> AcademicYearss { get; set; }
        public IEnumerable<SelectListItem> AcademicYears { get; set; }



    }
}
