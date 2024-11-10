using RSCS.FinancingStatements.Security.Authorization;
using RSCS.FinancingStatements.Security.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSCS.FinancingStatements.Security.Base
{
	public abstract class BaseController : Microsoft.AspNetCore.Mvc.Controller
	{
		protected virtual new CustomClaimPrincipal AppUser
		{
			get
			{
				return PageAccessManager.GetFullCustomPrincipal(Request.HttpContext);
			}
		}
	}
}
