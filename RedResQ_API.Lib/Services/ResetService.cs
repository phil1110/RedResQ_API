using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Models;

namespace RedResQ_API.Lib.Services
{
    public static class ResetService
    {
        public static bool RequestReset(JwtClaims claims, string email)
        {
            if(PermissionService.IsPermitted("requestReset", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Rr_RequestPasswordReset";

                parameters.Add(new SqlParameter { ParameterName = "@personEmail", SqlDbType = SqlDbType.VarChar, Value = email });

                int confirmationCode = (int)SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray()).Rows[0].ItemArray[0]!;

                EmailHandler.SendEmail(email, EmailHandler.GetEmail(confirmationCode));

                return true;
            }

            return false;
        }

        public static bool ConfirmReset(JwtClaims claims, int confirmationCode, string email, string password)
        {
            if (PermissionService.IsPermitted("confirmReset", claims.Role))
            {
                string hash = AuthService.HashPassword(password);

                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Rr_ConfirmPasswordReset";

                parameters.Add(new SqlParameter { ParameterName = "@confirmationCode", SqlDbType = SqlDbType.Int, Value = confirmationCode });
                parameters.Add(new SqlParameter { ParameterName = "@personEmail", SqlDbType = SqlDbType.VarChar, Value = email });
                parameters.Add(new SqlParameter { ParameterName = "@hash", SqlDbType = SqlDbType.VarChar, Value = hash });

                int response = (int)SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray()).Rows[0].ItemArray[0]!;

                if (response == 0)
                {
                    return false;
                }
            }

            return false;
        }

        public static bool CheckValidity(JwtClaims claims, int confirmationCode, string email)
        {
            if (PermissionService.IsPermitted("confirmReset", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Rr_CheckValidity";

                parameters.Add(new SqlParameter { ParameterName = "@confirmationCode", SqlDbType = SqlDbType.Int, Value = confirmationCode });
                parameters.Add(new SqlParameter { ParameterName = "@personEmail", SqlDbType = SqlDbType.VarChar, Value = email });

                int response = (int)SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray()).Rows[0].ItemArray[0]!;

                if (response == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
