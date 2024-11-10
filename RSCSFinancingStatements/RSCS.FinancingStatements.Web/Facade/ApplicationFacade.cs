using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RSCS.FinancingStatements.Web.Common;
using RSCS.FinancingStatements.Web.HttpHelper;
using RSCS.FinancingStatements.Web.Models;
using System.Security.Cryptography.Xml;
using System.Security.Principal;

namespace RSCS.FinancingStatements.Web.Facade
{
    public class ApplicationFacade : IApplicationFacade
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IConfiguration configuration;
        private readonly string ApiKey;
        public ApplicationFacade(IConfiguration configuration)
        {
            _httpClientHelper = new HttpClientHelper();
            ApiKey = configuration["ApiKey"];
        }

        public bool SetBearerToken(string token)
        {
            var headers = new Dictionary<string, string> { };
            headers.Add("Authorization", $"Bearer {token}");
            _httpClientHelper.SetHeaders(headers);
            return true;
        }

        //public CustomPrincipalClaim GetAccessToken(WindowsIdentity windowsIdentity)
        //{
        //    var authResult = new CustomPrincipalClaim();
        //    if (windowsIdentity != null)
        //    {
        //        WindowsIdentity.RunImpersonated(windowsIdentity.AccessToken, () =>
        //        {
        //            authResult = _httpClientHelper.PostRequest<AuthRequest, CustomPrincipalClaim>(string.Format(RSCSAPIUrlFactory.RSCSAPITokenEndpoint), new AuthRequest(), new CancellationToken()).Result;
        //        });
        //    }
        //    return authResult;
        //}
        public CustomPrincipalClaim GetAccessToken()
        {
            var authResult = new CustomPrincipalClaim();

            var headers = new Dictionary<string, string>
    {
        { "X-API-KEY", ApiKey }
    };

            authResult = _httpClientHelper.PostRequest<AuthRequest, CustomPrincipalClaim>(
                string.Format(RSCSAPIUrlFactory.RSCSAPITokenEndpoint),
                new AuthRequest(),
                new CancellationToken(),
                headers
            ).Result;

            return authResult;
        }
        public IEnumerable<Setting> FetchSomeSettings()
        {
            return _httpClientHelper.GetMultipleItemsRequest<Setting>(string.Format(RSCSAPIUrlFactory.FetchSomeSettings), new CancellationToken()).Result;
        }

        public User FetchUserByUsername(string username)
        {
            return _httpClientHelper.GetSingleItemRequest<User>(string.Format(RSCSAPIUrlFactory.FetchUserByUsername, username), new CancellationToken()).Result;
        }

        public User CreateUser(User user)
        {
            return _httpClientHelper.PostRequest<User, User>(string.Format(RSCSAPIUrlFactory.CreateUser), user, new CancellationToken()).Result;
        }
        public async Task<IEnumerable<FinPrograms>> FetchFinPrograms()
        {
            return _httpClientHelper.GetMultipleItemsRequest<FinPrograms>(string.Format(RSCSAPIUrlFactory.FetchFinPrograms), new CancellationToken()).Result;
        }

        public async Task<IEnumerable<FinProgramsFranchisee>> FetchFinProgramsFranchisee(int ProgramId)
        {
            return _httpClientHelper.GetMultipleItemsRequest<FinProgramsFranchisee>(string.Format(RSCSAPIUrlFactory.FetchFinProgramsFranchisee + "?ProgramId=" + ProgramId)).Result;
        }

        public async Task<IEnumerable<InvoiceDetails>> FetchInvoiceDetails(int ProgramId, string masterId)
        {
            return _httpClientHelper.GetMultipleItemsRequest<InvoiceDetails>(string.Format(RSCSAPIUrlFactory.FetchInvoiceDetails + "?ProgramId=" + ProgramId + "&masterId=" + masterId)).Result;
        }

        public async Task<IEnumerable<StatementDetails>> FetchStatementDetails(int ProgramId, string masterId)
        {
            return _httpClientHelper.GetMultipleItemsRequest<StatementDetails>(string.Format(RSCSAPIUrlFactory.FetchStatementDetails + "?ProgramId=" + ProgramId + "&masterId=" + masterId)).Result;
        }
        public async Task<IEnumerable<FinPrograms>> FetchProgramMaintenanceFinPrograms()
        {
            return _httpClientHelper.GetMultipleItemsRequest<FinPrograms>(string.Format(RSCSAPIUrlFactory.FetchProgramMaintenanceFinPrograms), new CancellationToken()).Result;
        }
        public async Task<int> UpdateFinPrograms(FinProgramRequestParameter finProgramRequestParameter)
        {

            return _httpClientHelper.PutRequest<FinProgramRequestParameter, int>(string.Format(RSCSAPIUrlFactory.UpdateFinProgrm), finProgramRequestParameter, new CancellationToken()).Result;
        }
        public async Task<IEnumerable<References>> FetchSecurityFinProgFranchisee()
        {
            return _httpClientHelper.GetMultipleItemsRequest<References>(string.Format(RSCSAPIUrlFactory.FetchSecurityFinProgFranchisee), new CancellationToken()).Result;
        }
        public async Task<IEnumerable<Contacts>> FetchSecurityContacts(int MasterID)
        {
            string s = string.Format(RSCSAPIUrlFactory.FetchSecurityContacts + "? MasterID = " + MasterID + "");
            return _httpClientHelper.GetMultipleItemsRequest<Contacts>(string.Format(RSCSAPIUrlFactory.FetchSecurityContacts + "?MasterID=" + MasterID + ""), new CancellationToken()).Result;
        }
        public async Task<int> GrantOrRestrictAccess(AccesResquestParameter accesResquestParameter)
        {

            return _httpClientHelper.PutRequest<AccesResquestParameter, int>(string.Format(RSCSAPIUrlFactory.GrantOrRestrictAccess), accesResquestParameter, new CancellationToken()).Result;
        }
    }
}
