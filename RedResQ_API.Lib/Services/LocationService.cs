using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
    public static class LocationService
    {
        public static Location Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_GetLocation";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable locationTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (locationTable.Rows.Count == 1)
            {
                return Converter.ToLocation(locationTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException("No Location was found!");
        }

        public static long Search(string city, string postalCode, long countryId)
        {
            List<Location> locations = new List<Location>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_SearchForLocation";

            parameters.Add(new SqlParameter { ParameterName = "@city", SqlDbType = SqlDbType.VarChar, Value = city });
            parameters.Add(new SqlParameter { ParameterName = "@postalCode", SqlDbType = SqlDbType.VarChar, Value = postalCode });
            parameters.Add(new SqlParameter { ParameterName = "@countryId", SqlDbType = SqlDbType.BigInt, Value = countryId });

            DataTable locationTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (locationTable.Rows.Count == 1)
            {
                return Convert.ToInt64(locationTable.Rows[0].ItemArray[0]);
            }

            return -1;
        }

        public static Location[] Fetch(JwtClaims claims, long? id)
        {
            List<Location> locations = new List<Location>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_GetLocations";

            if (id.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id.Value });
            }

            DataTable locationTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (locationTable.Rows.Count > 0)
            {
                foreach (DataRow row in locationTable.Rows)
                {
                    locations.Add(Converter.ToLocation(row.ItemArray.ToList()!));
                }

                return locations.ToArray();
            }

            throw new NotFoundException("No Locations were found!");
        }

        public static bool Add(string city, string postalCode, long countryId)
        {
            List<Location> locations = new List<Location>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_AddLocation";

            parameters.Add(new SqlParameter { ParameterName = "@city", SqlDbType = SqlDbType.VarChar, Value = city });
            parameters.Add(new SqlParameter { ParameterName = "@postalCode", SqlDbType = SqlDbType.VarChar, Value = postalCode });
            parameters.Add(new SqlParameter { ParameterName = "@countryId", SqlDbType = SqlDbType.BigInt, Value = countryId });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }

        public static bool Edit(Location location)
        {
            List<Location> locations = new List<Location>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_EditLocation";
            Location oldLoc = Get(location.Id);

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = location.Id });

            if (location.City != oldLoc.City)
            {
                parameters.Add(new SqlParameter { ParameterName = "@city", SqlDbType = SqlDbType.VarChar, Value = location.City });
            }
            if (location.PostalCode != oldLoc!.PostalCode)
            {
                parameters.Add(new SqlParameter { ParameterName = "@postalCode", SqlDbType = SqlDbType.VarChar, Value = location.PostalCode });
            }
            if (location.Country.Id != oldLoc!.Country.Id)
            {
                parameters.Add(new SqlParameter { ParameterName = "@countryId", SqlDbType = SqlDbType.BigInt, Value = location.Country.Id });
            }

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            switch (parameters.Count)
            {
                case 2:
                    if (rowsAffected == 1)
                    {
                        return true;
                    }
                    break;
                case 3:
                    if (rowsAffected == 2)
                    {
                        return true;
                    }
                    break;
                case 4:
                    if (rowsAffected == 3)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public static bool Delete(long id)
        {
            List<Location> locations = new List<Location>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_DeleteLocation";

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
