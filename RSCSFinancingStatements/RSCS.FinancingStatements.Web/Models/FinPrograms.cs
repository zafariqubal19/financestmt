using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RSCS.FinancingStatements.Web.Models
{
    public class FinPrograms
    {
        [JsonProperty]
        public int ProgramID { get; set; }
        [JsonProperty]
        public string CampaignCode { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public DateTime StartDate { get; set; }
        [JsonProperty]
        public DateTime EndDate { get; set; }
        [JsonProperty]
        [Display(Name ="Terms")]
        public string PaymentTermsCode { get; set; }
        [JsonProperty]
        public DateTime DateAdded { get; set; }
        [JsonProperty]
        public string ActiveProcess { get; set; }
        [JsonProperty]
        public string ActiveWeb { get; set; }
        [JsonProperty]
        public string Comments { get; set; }
        [JsonProperty]
        [Display(Name ="#Fran Inv")]
        public int FranchiseeCount { get; set; }
        [JsonProperty]
        [Display(Name ="#Inv")]
        public int InvoiceCount { get; set; }
        [JsonProperty]
        [Display(Name ="#Stamt")]
        public int StatementCount { get; set; }
        [JsonProperty]
        [Display(Name ="#Stores")]
        public int StoreCount { get; set; }
        [JsonProperty]
        [Display(Name ="#Fran Stmt")]
        public int StatementFranchiseeCount { get; set; }
        [JsonProperty]
        [Display(Name ="To Stmt")]
        public int LastStatementPeriod { get; set; }
        [JsonProperty]
        [Display(Name ="From Stmt")]
        public int FirstStatementPeriod { get; set; }
        [JsonProperty]
        [Display(Name ="From Inv Date")]
        public DateTime FirstInvoice { get; set; }

        [JsonProperty]
        [Display(Name ="To Inv Date")]
        public DateTime lastInvoice { get; set; }
    }
}
