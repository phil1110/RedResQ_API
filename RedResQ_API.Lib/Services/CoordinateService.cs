using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedResQ_API.Lib.Services
{
    public static class CoordinateService
    {
        public static bool LogCoordinates(JwtClaims claims, float lat, float lon, string token)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Cd_LogCoordinates";

            parameters.Add(new SqlParameter { ParameterName = "@userId", SqlDbType = SqlDbType.BigInt, Value = claims.Id });
            parameters.Add(new SqlParameter { ParameterName = "@latitude", SqlDbType = SqlDbType.Float, Value = lat });
            parameters.Add(new SqlParameter { ParameterName = "@longitude", SqlDbType = SqlDbType.Float, Value = lon });
            parameters.Add(new SqlParameter { ParameterName = "@token", SqlDbType = SqlDbType.VarChar, Value = token });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }
    }
}
