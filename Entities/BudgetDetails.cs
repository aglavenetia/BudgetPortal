using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace BudgetPortal.Entities
{
    public class BudgetDetails
    {
        public int id { get; set; }

        [Required]
        [ForeignKey("DivisionID")]
        public ICollection<Divisions> DivID { get; set; }

        [Required]
        public int FinancialYear1 { get; set; }

        [Required]
        public int FinancialYear2 { get; set; }

        [Required]
        [ForeignKey("HeadNo")]
        public ICollection<HeadDetails> HeadNo { get; set; }

        [Required]
        [ForeignKey("SubHeadNo")]
        public ICollection<SubHeadDetails> SubHeadNo { get; set; }

        public SqlMoney BudEstCurrFin { get; set; }

        public SqlMoney ActPrevFin { get; set; }

        public SqlMoney ActCurrFinTill2ndQuart { get; set; }

        public SqlMoney RevEstCurrFin { get; set; }

        public Decimal PerVarRevEstOverBudgEstCurrFin { get; set; }

        public SqlMoney ACAndBWPropRECurrFin { get; set;}

        public SqlMoney BudgEstNexFin { get; set; }

        public Decimal PerVarRevEstOverBudgEstNxtFin { get; set; }

        public SqlMoney ACAndBWPropRENxtFin { get; set; }

        public string Justification { get; set; }

    }
}
