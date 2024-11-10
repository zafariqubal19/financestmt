using System.ComponentModel.DataAnnotations;

namespace RSCS.FinancingStatements.Web.Models
{
    public class StatementDetails
    {
        public string Program { get; set; }
        public string Statement { get; set; }
        [Display(Name ="File")]
        public string FileName { get; set; }
        [Display(Name ="Path")]
        public string FilePath { get; set; }
        public DateTime DateAdded { get; set; }
        public string QARoot { get; set; }
        public string ProdRoot { get; set; }
        public int StatementID { get; set; }
        public int ProgramID { get; set; }
        public string FranchiseeID { get; set; }
        public string MasterID { get; set; }
    }
}
