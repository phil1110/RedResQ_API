using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics.Metrics;

namespace RedResQ_API.Lib.Services
{
    public static class CountryService
    {
        public static Country[] GetAllCountries(JwtClaims claims)
        {
            if(PermissionService.IsPermitted("getCountry", claims.Role))
            {
                List<Country> countries = new List<Country>();
                string storedProcedure = "SP_Co_GetCountries";

                DataTable countryTable = SqlHandler.ExecuteQuery(storedProcedure);

                if (countryTable.Rows.Count > 0)
                {
                    foreach (DataRow row in countryTable.Rows)
                    {
                        countries.Add(Converter.ToCountry(row.ItemArray.ToList()!));
                    }

                    return countries.ToArray();
                }
            }

            throw new Exception("No Countries were found!");
        }

        public static Country GetCountry(JwtClaims claims, long id)
        {
            if(PermissionService.IsPermitted("getCountry", claims.Role))
            {
                List<Country> countries = new List<Country>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Co_GetCountry";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                DataTable countryTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (countryTable.Rows.Count == 1)
                {
                    return Converter.ToCountry(countryTable.Rows[0].ItemArray.ToList()!);
                }
            }

            throw new Exception("Row count was not 1!");
        }

        public static bool AddCountry(JwtClaims claims, string name)
        {
            if(PermissionService.IsPermitted("addCountry", claims.Role))
            {
                List<Country> countries = new List<Country>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Co_AddCountry";

                parameters.Add(new SqlParameter { ParameterName = "@countryName", SqlDbType = SqlDbType.VarChar, Value = name });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if(rowsAffected == 1)
                {
                    return true;
                }

                return false;
            }

            throw new Exception("Not permitted to complete this action!");
        }

        public static bool AddCountryArray(JwtClaims claims, string[] names)
        {
            bool result = true;

            foreach (string name in names)
            {
                if (result)
                {
                    result = AddCountry(claims, name);
                }
                else
                {
                    return result;
                }
            }

            return true;
        }

        public static bool EditCountry(JwtClaims claims, Country country)
        {
            if(PermissionService.IsPermitted("editCountry", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Co_EditCountry";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = country.Id });
                parameters.Add(new SqlParameter { ParameterName = "@countryName", SqlDbType = SqlDbType.VarChar, Value = country.CountryName });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }

                return false;
            }

            throw new Exception("Not permitted to complete this action!");
        }

        public static bool DeleteCountry(JwtClaims claims, long id)
        {
            if (PermissionService.IsPermitted("deleteCountry", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Co_DeleteCountry";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }

                return false;
            }

            throw new Exception("Not permitted to complete this action!");
        }
    }
}
