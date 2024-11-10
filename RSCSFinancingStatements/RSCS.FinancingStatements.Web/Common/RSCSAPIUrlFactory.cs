using Microsoft.Extensions.Configuration;
namespace RSCS.FinancingStatements.Web.Common
{

    public static class RSCSAPIUrlFactory
    {
        private static IConfiguration _configuration;

        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string RSCSAPIBaseUrl
        {
            get { return _configuration["RSCSAPIBaseUrl"]; }
        }
        //public static string RSCSAPIBaseUrl { get { return $"http://localhost:5139/api"; } }
        public static string RSCSAPITokenEndpoint { get { return $"{RSCSAPIBaseUrl}/identity/token"; } }

        public static string FetchSomeSettings { get { return $"{RSCSAPIBaseUrl}/setting/some-settings"; } }
        public static string FetchUserByUsername { get { return $"{RSCSAPIBaseUrl}/user/user-by-username?username={{0}}"; } }
        public static string CreateUser { get { return $"{RSCSAPIBaseUrl}/user/create-user"; } }
        public static string FetchFinPrograms { get { return $"{RSCSAPIBaseUrl}/Invoice/getFinPrograms"; } }
        public static string FetchFinProgramsFranchisee { get { return $"{RSCSAPIBaseUrl}/Invoice/getFinProgramsFranchisee"; } }
        public static string FetchInvoiceDetails { get { return $"{RSCSAPIBaseUrl}/Invoice/getInvoiceDetails"; } }
        public static string FetchStatementDetails { get { return $"{RSCSAPIBaseUrl}/Invoice/getStatementDetails"; } }
        public static string FetchProgramMaintenanceFinPrograms { get { return $"{RSCSAPIBaseUrl}/ProgramMaintenance/getFinPrograms"; } }
        public static string UpdateFinProgrm { get { return $"{RSCSAPIBaseUrl}/ProgramMaintenance/UpdateFinPrograms"; } }
        public static string FetchSecurityFinProgFranchisee { get { return $"{RSCSAPIBaseUrl}/Security/getReference"; } }
        public static string FetchSecurityContacts { get { return $"{RSCSAPIBaseUrl}/Security/getContacts"; } }
        public static string GrantOrRestrictAccess { get { return $"{RSCSAPIBaseUrl}/Security/grantOrRestrictAccess"; } }
    }

}
