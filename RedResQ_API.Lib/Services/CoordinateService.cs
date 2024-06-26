﻿using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
    public static class CoordinateService
    {
        public static bool LogCoordinates(JwtClaims claims, double lat, double lon, string token)
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


        public static string[] GetTokens(double lat, double lon, int radius)
        {
            List<string> tokens = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Cd_GetTokens";

            parameters.Add(new SqlParameter { ParameterName = "@lat", SqlDbType = SqlDbType.Float, Value = lat });
            parameters.Add(new SqlParameter { ParameterName = "@lon", SqlDbType = SqlDbType.Float, Value = lon });
            parameters.Add(new SqlParameter { ParameterName = "@radius", SqlDbType = SqlDbType.Int, Value = radius });

            DataTable tokenTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (tokenTable.Rows.Count > 0)
            {
                foreach (DataRow row in tokenTable.Rows)
                {
                    tokens.Add(Convert.ToString(row.ItemArray[0])!);
                }

                return tokens.ToArray();
            }

            return null!;
        }
    }
}
