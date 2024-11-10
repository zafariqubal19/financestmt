using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RSCS.FinancingStatements.Web.Models
{
    public class FinProgramsUpdate
    {
        [JsonProperty]
        public int ProgramID { get; set; }
        [JsonProperty]
        public DateTime StartDate { get; set; }
        [JsonProperty]
        public DateTime EndDate { get; set; }
        [JsonProperty]
        [Display(Name = "Terms")]
        public string PaymentTermsCode { get; set; }
        [JsonProperty]
        public string ActiveProcess { get; set; }
        [JsonProperty]
        public string ActiveWeb { get; set; }
        [JsonProperty]
        public string Comments { get; set; }
    }
}
