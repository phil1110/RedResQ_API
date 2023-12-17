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
        /// <summary>
        /// Returns a role from the database based on the given identifier
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="id">The identifier given to define the targeted dataset</param>
        /// <returns>The target role</returns>
        /// <exception cref="Exception">Is thrown, when no or more than one role was found</exception>
        public static Role Get(JwtClaims claims, long id)
		{
			if(PermissionService.IsPermitted("getRole", claims.Role))
			{
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ro_GetRole";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                DataTable roleTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (roleTable.Rows.Count == 1)
                {
                    return Role.ConvertToRole(roleTable.Rows[0]);
                }
            }

            throw new Exception("Role was not found!");
        }

        /// <summary>
        /// Fetches all roles from the database
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <returns>An array of Role objects containing all available Roles</returns>
        /// <exception cref="Exception">Is thrown when no roles where found in the database</exception>
        public static Role[] Fetch(JwtClaims claims)
        {
			if(PermissionService.IsPermitted("fetchRoles", claims.Role))
			{
                List<Role> roles = new List<Role>();
                string storedProcedure = "SP_Ro_GetAllRoles";

                DataTable roleTable = SqlHandler.ExecuteQuery(storedProcedure);

                if (roleTable.Rows.Count > 0)
                {
                    foreach (DataRow row in roleTable.Rows)
                    {
                        roles.Add(Role.ConvertToRole(row));
                    }

                    return roles.ToArray();
                }
            }

            throw new Exception("Role was not found!");
        }
    }
}
