
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RSCS.FinancingStatements.Security.Principal
{
	public class CustomClaimPrincipal : ICustomClaimPrincipal
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string AccessToken { get; set; }
		public string AccessRoleName { get; set; }
		public string SessionID { get; set; }

		public IIdentity? Identity { get; private set; }

		public bool IsInRole(string role)
		{
			return role.ToUpper().Trim() == AccessRoleName.ToUpper().Trim();
		}

		public CustomClaimPrincipal(string userName)
		{
			this.Username = userName;
			this.Identity = new GenericIdentity(userName);
		}
	}
}
