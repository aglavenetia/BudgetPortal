using BudgetPortal.Entities;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace BudgetPortal.Models
{
    public class JoinedModel
    {
        public BudgetSections section { get; set; }
        public BudgetGroups group { get; set; }

    }
}
