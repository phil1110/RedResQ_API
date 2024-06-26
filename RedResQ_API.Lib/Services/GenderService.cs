﻿using RedResQ_API.Lib.Exceptions;
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
        public static Gender[] GetAll()
        {
            List<Gender> genders = new List<Gender>();
            string storedProcedure = "SP_Ge_GetAllGenders";

            DataTable genderTable = SqlHandler.ExecuteQuery(storedProcedure);

            if (genderTable.Rows.Count > 0)
            {
                foreach (DataRow row in genderTable.Rows)
                {
                    genders.Add(Converter.ToGender(row.ItemArray.ToList()!));
                }

                return genders.ToArray();
            }

            throw new NotFoundException("No Genders were found!");
        }

        public static Gender Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ge_GetGender";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable genderTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (genderTable.Rows.Count == 1)
            {
                return Converter.ToGender(genderTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException("No Genders were found!");
        }

        public static bool Add(JwtClaims claims, string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ge_AddGender";

            parameters.Add(new SqlParameter { ParameterName = "@genderName", SqlDbType = SqlDbType.VarChar, Value = name });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }

        public static bool Edit(Gender gender)
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

            return false;
        }

        public static bool Delete(JwtClaims claims, long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ge_DeleteGender";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }
    }
}
