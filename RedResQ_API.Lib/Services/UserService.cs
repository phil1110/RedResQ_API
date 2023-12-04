using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public static class UserService
    {
        public static bool RequestReset(string email)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Rr_RequestPasswordReset";

            parameters.Add(new SqlParameter { ParameterName = "@personEmail", SqlDbType = SqlDbType.VarChar, Value = email });

            int confirmationCode = (int)SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray()).Rows[0].ItemArray[0]!;

            EmailService.SendEmail(email, EmailService.GetEmail(confirmationCode));

            return true;
        }

        public static bool ConfirmReset(int confirmationCode, string email, string password)
        {
            string hash = SessionService.HashPassword(password);

            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Rr_ConfirmPasswordReset";

            parameters.Add(new SqlParameter { ParameterName = "@confirmationCode", SqlDbType = SqlDbType.Int, Value = confirmationCode });
            parameters.Add(new SqlParameter { ParameterName = "@personEmail", SqlDbType = SqlDbType.VarChar, Value = email });
            parameters.Add(new SqlParameter { ParameterName = "@hash", SqlDbType = SqlDbType.VarChar, Value = hash });

            int response = (int)SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray()).Rows[0].ItemArray[0]!;

            if(response == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
