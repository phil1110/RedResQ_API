using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedResQ_API.Lib.Services
{
    public static class GenderService
    {
        public static Gender[] GetAll(JwtClaims claims)
        {
            if (PermissionService.IsPermitted("getGender", claims.Role))
            {
                List<Gender> genders = new List<Gender>();
                string storedProcedure = "SP_Ge_GetAllGenders";

                DataTable genderTable = SqlHandler.ExecuteQuery(storedProcedure);

                if (genderTable.Rows.Count > 0)
                {
                    foreach (DataRow row in genderTable.Rows)
                    {
                        genders.Add(Gender.ConvertToGender(row));
                    }

                    return genders.ToArray();
                }
            }

            throw new Exception("No Genders were found!");
        }

        public static Gender Get(long id)
        {
            List<Gender> genders = new List<Gender>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ge_GetGender";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable genderTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (genderTable.Rows.Count == 1)
            {
                return Gender.ConvertToGender(genderTable.Rows[0]);
            }

            throw new Exception("Row count was not 1!");
        }

        public static bool Add(JwtClaims claims, string name)
        {
            if (PermissionService.IsPermitted("addGender", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ge_AddGender";

                parameters.Add(new SqlParameter { ParameterName = "@genderName", SqlDbType = SqlDbType.VarChar, Value = name });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Edit(JwtClaims claims, Gender gender)
        {
            if (PermissionService.IsPermitted("editGender", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ge_EditGender";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = gender.Id });
                parameters.Add(new SqlParameter { ParameterName = "@genderName", SqlDbType = SqlDbType.VarChar, Value = gender.Name });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Delete(JwtClaims claims, long id)
        {
            if (PermissionService.IsPermitted("deleteGender", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ge_DeleteGender";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
