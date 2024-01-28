using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Models;
using System.Reflection.Metadata.Ecma335;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
    public static class PermissionService
    {
        /// <summary>
        /// Determines the requested Permission
        /// </summary>
        /// <param name="permissionName">The identifier of the targeted Permission</param>
        /// <returns>The requested Permission</returns>
        public static Permission GetPermission(JwtClaims claims, string permissionName)
        {
            if (IsPermitted("getPermission", claims.Role))
            {
                return GetPermission(permissionName);
            }

            return null!;
        }

        private static Permission GetPermission(string permissionName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pm_GetPermission";

            parameters.Add(new SqlParameter { ParameterName = "@permissionName", SqlDbType = SqlDbType.VarChar, Value = permissionName });

            DataTable permissionTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            return Converter.ToPermission(permissionTable.Rows[0].ItemArray.ToList()!);
        }

        /// <summary>
        /// Checks if a certain Role is allowed to execute an action
        /// </summary>
        /// <param name="permissionName">The name of the Permission in question</param>
        /// <param name="roleId">The Identifier of the Role</param>
        /// <returns>true if the Role is allowed to complete the action, false if not</returns>
        public static bool IsPermitted(string permissionName, long roleId)
        {
            Permission permission = GetPermission(permissionName);

            if(permission.RequiredRole.Id <= roleId)
            {
                return true;
            }

            throw new AuthException("Required role was " + permission.RequiredRole.Name + "! Not authorised to complete this action!");
        }

        /// <summary>
        /// Returns of existing Permissions
        /// </summary>
        /// <returns>An array of all Permissions</returns>
        public static Permission[] GetAllPermissions(JwtClaims claims)
        {
            if(IsPermitted("getPermission", claims.Role))
            {
                List<Permission> permissions = new List<Permission>();
                string storedProcedure = "SP_Pm_GetAllPermissions";

                DataTable roleTable = SqlHandler.ExecuteQuery(storedProcedure);

                if (roleTable.Rows.Count > 0)
                {
                    foreach (DataRow row in roleTable.Rows)
                    {
                        permissions.Add(Converter.ToPermission(row.ItemArray.ToList()!));
                    }

                    return permissions.ToArray();
                }
            }

            return null!;
        }

        /// <summary>
        /// Gets all permitted actions for a specific Role
        /// </summary>
        /// <param name="roleId">The identifier of the role in question</param>
        /// <returns>An array of all permitted actions</returns>
        public static Permission[] GetAllPermissionsForRole(JwtClaims claims, long roleId)
        {
            if (IsPermitted("getPermission", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Pm_GetAllPermissionsForRole";

                parameters.Add(new SqlParameter { ParameterName = "@roleId", SqlDbType = SqlDbType.BigInt, Value = roleId });

                DataTable roleTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (roleTable.Rows.Count > 0)
                {
                    List<Permission> permissions = new List<Permission>();

                    foreach (DataRow row in roleTable.Rows)
                    {
                        permissions.Add(Converter.ToPermission(row.ItemArray.ToList()!));
                    }

                    return permissions.ToArray();
                }
            }

            return null!;
        }

        /// <summary>
        /// Updates the required Role of a specific Permission
        /// </summary>
        /// <param name="permissionName">Identifier of the targeted permission</param>
        /// <param name="roleId">Identifier of the new required role</param>
        /// <returns>The number of affected rows in the database</returns>
        public static int UpdatePermission(JwtClaims claims, string permissionName, long roleId)
        {
            if(IsPermitted("updatePermission", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Pm_GetAllPermissionsForRole";

                parameters.Add(new SqlParameter { ParameterName = "@permissionName", SqlDbType = SqlDbType.VarChar, Value = permissionName });
                parameters.Add(new SqlParameter { ParameterName = "@roleId", SqlDbType = SqlDbType.BigInt, Value = roleId });

                return SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());
            }
            else
            {
                throw new Exception("Not authorized");
            }
        }
    }
}
