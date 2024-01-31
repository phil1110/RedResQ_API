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
        public static Permission GetPermission(string permissionName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pm_GetPermission";

            parameters.Add(new SqlParameter { ParameterName = "@permissionName", SqlDbType = SqlDbType.VarChar, Value = permissionName });

            DataTable permissionTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            return Converter.ToPermission(permissionTable.Rows[0].ItemArray.ToList()!);
        }

        public static bool IsPermitted(string permissionName, long roleId)
        {
            Permission permission = GetPermission(permissionName);

            if(permission.RequiredRole.Id <= roleId)
            {
                return true;
            }

            throw new AuthException("Required role was " + permission.RequiredRole.Name + "! Not authorised to complete this action!");
        }

        public static Permission[] GetAllPermissions()
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

            return null!;
        }

        public static Permission[] GetAllPermissionsForRole(long roleId)
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

            return null!;
        }

        public static int UpdatePermission(string permissionName, long roleId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pm_GetAllPermissionsForRole";

            parameters.Add(new SqlParameter { ParameterName = "@permissionName", SqlDbType = SqlDbType.VarChar, Value = permissionName });
            parameters.Add(new SqlParameter { ParameterName = "@roleId", SqlDbType = SqlDbType.BigInt, Value = roleId });

            return SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());
        }
    }
}
