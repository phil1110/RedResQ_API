using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
	public static class RoleService
	{
        public static Role Get(long id)
		{
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ro_GetRole";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable roleTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (roleTable.Rows.Count == 1)
            {
                return Converter.ToRole(roleTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException("Role was not found!");
        }

        public static Role[] Fetch()
        {
            List<Role> roles = new List<Role>();
            string storedProcedure = "SP_Ro_GetAllRoles";

            DataTable roleTable = SqlHandler.ExecuteQuery(storedProcedure);

            if (roleTable.Rows.Count > 0)
            {
                foreach (DataRow row in roleTable.Rows)
                {
                    roles.Add(Converter.ToRole(row.ItemArray.ToList()!));
                }

                return roles.ToArray();
            }

            throw new NotFoundException("Role was not found!");
        }
    }
}
