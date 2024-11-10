namespace RSCS.FinancingStatements.Web.Models
{
    public class FinProgramRequestParameter
    {
        public int ProgramId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ActiveProcess { get; set; } = null!;
        public string? ActiveWeb { get; set; } = null!;
        public string PaymentTermsCode { get; set; }
        public string Comments { get; set; }
    }
}
