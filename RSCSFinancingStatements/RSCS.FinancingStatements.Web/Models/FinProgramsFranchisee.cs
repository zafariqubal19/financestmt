using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace RSCS.FinancingStatements.Web.Models
{
    public class FinProgramsFranchisee
    {
        public int ProgramID { get; set; }
        public string MasterID { get; set; }
        public string Reference { get; set; }
        [Display(Name ="Amount")]
        public float InvoiceAmount { get; set; }
        [Display(Name ="Stores")]
        public int InvoiceStores { get; set; }
        public int Invoices { get; set; }
        [Display(Name ="From Inv Date")]
        public DateTime InvoiceDateFrom { get; set; }
        [Display(Name ="To Inv Date")]
        public DateTime InvoiceDateTo { get; set; }
        public int Statements { get; set; }
        [Display(Name ="From Stmt")]
        public int StatementFrom { get; set; }
        [Display(Name ="To Stmt")]
        public int StatementTo { get; set; }
    }
}
