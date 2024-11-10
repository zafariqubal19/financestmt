
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSCS.FinancingStatements.Security.Principal
{
	public class CustomClaimPrincipalSerializeModel
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string AccessToken { get; set; }
		public string AccessRoleName { get; set; }
	}
}
