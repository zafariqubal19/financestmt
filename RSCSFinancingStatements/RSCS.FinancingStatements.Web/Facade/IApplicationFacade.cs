using RSCS.FinancingStatements.Web.Models;
using System.Security.Cryptography.Xml;
using System.Security.Principal;

namespace RSCS.FinancingStatements.Web.Facade
{
    public interface IApplicationFacade
    {
        bool SetBearerToken(string token);
        CustomPrincipalClaim GetAccessToken();
        IEnumerable<Setting> FetchSomeSettings();
        User FetchUserByUsername(string username);
        User CreateUser(User user);
        Task<IEnumerable<FinPrograms>> FetchFinPrograms();
        Task<IEnumerable<FinProgramsFranchisee>> FetchFinProgramsFranchisee(int ProgramId);
        Task<IEnumerable<InvoiceDetails>> FetchInvoiceDetails(int ProgramId, string masterId);
        Task<IEnumerable<StatementDetails>> FetchStatementDetails(int ProgramId, string masterId);
        Task<IEnumerable<FinPrograms>> FetchProgramMaintenanceFinPrograms();
        Task<int> UpdateFinPrograms(FinProgramRequestParameter finProgramRequestParameter);
        Task<IEnumerable<References>> FetchSecurityFinProgFranchisee();
        Task<IEnumerable<Contacts>> FetchSecurityContacts(int MasterID);
        Task<int> GrantOrRestrictAccess(AccesResquestParameter accesResquestParameter);
    }
}
