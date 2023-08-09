using BudgetPortal.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace BudgetPortal.ViewModel
{
    public class MultipleData
    {
        /* public int SectionNo { get; set; }
         public String SectionName { get; set; }
         public String GroupNo { get; set; }
         public String GroupName { get; set; }

         public String SubGroupNo { get; set; }

         public String SubGroupName { get; set; }

         public String LedgerNumber { get; set; }

         public String LedgerName { get; set; }*/

        public IEnumerable<BudgetSections> Sectionss { get; set; }
        public IEnumerable<BudgetGroups> Groupss { get; set; }

        public IEnumerable<BudgetSubGroups> SubGroupss { get; set; }

        public IEnumerable<BudgetLedgers> Ledgerss { get; set; }

        public IEnumerable<Division> Divisionss { get; set; }

    }
}
