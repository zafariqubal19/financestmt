﻿using Entities = RSCS.FinancingStatements.Core.Models.EntityModels;
using RSCS.FinancingStatements.Core.Interfaces.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RSCS.FinancingStatements.Core.Interfaces.Repository
{
	public interface IUserRepository : IGenericRepository<Entities.tblUser>
	{

	}
}