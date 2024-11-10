using Microsoft.AspNetCore.Http;
using RSCS.FinancingStatements.Security.Extensions;
using RSCS.FinancingStatements.Security.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSCS.FinancingStatements.Security.Authorization
{
    public static class PageAccessManager
    {
        public static CustomClaimPrincipal GetFullCustomPrincipal(HttpContext httpContext)
        {
            CustomClaimPrincipalSerializeModel serializeModel = httpContext.Session.Get<CustomClaimPrincipalSerializeModel>("APP_USER_INFO");

            if (serializeModel == null) return null;

            CustomClaimPrincipal newUser = new CustomClaimPrincipal(serializeModel.Username);
            newUser.Username = serializeModel.Username;
            newUser.FirstName = serializeModel.FirstName;
            newUser.LastName = serializeModel.LastName;
            newUser.Email = serializeModel.Email;
            newUser.AccessRoleName = serializeModel.AccessRoleName;
            newUser.AccessToken = serializeModel.AccessToken;
            newUser.SessionID = httpContext.Session.Id;
            return newUser;
        }
    }
}
