using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
	public static class RoleService
	{
		public static Role GetRole(long id)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Ro_GetRole";

			parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

			DataTable roleTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if(roleTable.Rows.Count == 1)
			{
				return Role.ConvertToRole(roleTable.Rows[0]);
			}
			else
			{
				throw new Exception("Role was not found!");
			}
		}
	}
}
