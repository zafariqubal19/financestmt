namespace RSCS.FinancingStatements.Web.Models
{
   
    public class AuthorizationResponse
    {
        public APIResultObject result { get; set; }
        public bool succeeded { get; set; }
        public string error { get; set; }
    }
}
