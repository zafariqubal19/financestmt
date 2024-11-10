namespace RSCS.FinancingStatements.Web.Models
{
    public class AuthRequest
    {
    }
    public class CustomPrincipalClaim
    {
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; } = false;
        public string AccessToken { get; set; }
    }
}
