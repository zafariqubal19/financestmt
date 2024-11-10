using System.ComponentModel.DataAnnotations;

namespace RSCS.FinancingStatements.Web.Models
{
    public class InvoiceDetails
    {
        [Display(Name ="Invoice")]
        public string InvoiceNo { get; set; }
        [Display(Name ="Inv Date")]
        public DateTime InvoiceDate { get; set; }
        [Display(Name ="Amount")]
        public float TotalAmount { get; set; }
        [Display(Name ="Views")]
        public int ViewCount { get; set; }
        [Display(Name ="BillTo")]

        public string BilltoCustomerNo { get; set; }
        [Display(Name ="Billto Name")]
        public string BilltoName { get; set; }
        [Display(Name ="SellTo")]
        public string SelltoCustomerNo { get; set; }
        public string StoreID { get; set; }
        [Display(Name ="SellTo Name")]
        public string SelltoCustomerName { get; set; }
        [Display(Name ="Address")]
        public string SelltoAddress { get; set; }
        [Display(Name ="City")]
        public string SelltoCity { get; set; }
        [Display(Name ="ST")]
        public string SelltoState { get; set; }
        [Display(Name ="Zip")]
        public string SelltoZip { get; set; }
        public string OrigFilePath { get; set; }
        public string FilePath { get; set; }
        public DateTime DateAdded { get; set; }
        public int Copied { get; set; }
        public int ProgramID { get; set; }
        public string masterID { get; set; }
        public string FranchiseeID { get; set; }
    }
}
