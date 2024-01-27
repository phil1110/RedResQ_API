using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public static class LocationService
    {
        /// <summary>
        /// Gets one singular Location entity from the database
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="id">The identifier of the wanted Location entity</param>
        /// <returns>The targeted Location object</returns>
        /// <exception cref="Exception">Is thrown if there is more or less than 1 Rows in the received dataset from the SQL Server.</exception>
        public static Location Get(JwtClaims claims, long id)
        {
            if(PermissionService.IsPermitted("getLocation", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Lo_GetLocation";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                DataTable locationTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (locationTable.Rows.Count == 1)
                {
                    return Location.ConvertToLocation(locationTable.Rows[0]);
                }
            }

            throw new Exception("Row count was not 1!");
        }

        /// <summary>
        /// Searches for a Location entitiy, based on its content
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="city">The City name to look for</param>
        /// <param name="postalCode">The postal code to look for</param>
        /// <param name="countryId">The identifier of the country the location is in</param>
        /// <returns>The identifier of the targeted Location if found, otherwise -1</returns>
        public static long Search(JwtClaims claims, string city, string postalCode, long countryId)
        {
            if(PermissionService.IsPermitted("searchLocation", claims.Role))
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
                    return Converter.ToInt64(locationTable.Rows[0].ItemArray[0]);
                }
            }

            return -1;
        }

        /// <summary>
        /// Fetches ten entities of the Location table from the database
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="id">Optional parameter, to continue listing from this id</param>
        /// <returns>An array of Location objects</returns>
        /// <exception cref="Exception">Is thrown if there were no Rows in the received dataset.</exception>
        public static Location[] Fetch(JwtClaims claims, long? id)
        {
            if(PermissionService.IsPermitted("getLocation", claims.Role))
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
                        locations.Add(Location.ConvertToLocation(row));
                    }

                    return locations.ToArray();
                }
            }

            throw new Exception("No Locations were found!");
        }

        /// <summary>
        /// Adds a new Location entity to the database
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="city">The name of the city add</param>
        /// <param name="postalCode">The postal code to add</param>
        /// <param name="countryId">The identifier of the location's country</param>
        /// <returns>true if the location was successfully added, false if not</returns>
        public static bool Add(JwtClaims claims, string city, string postalCode, long countryId)
        {
            if(PermissionService.IsPermitted("addLocation", claims.Role))
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
            }

            return false;
        }

        /// <summary>
        /// Compares the given location to the one in the database and updates changed values
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="location">The updated location object</param>
        /// <returns>true if the location entity was successfully updated, false if not</returns>
        public static bool Edit(JwtClaims claims,  Location location)
        {
            if(PermissionService.IsPermitted("editLocation", claims.Role))
            {
                List<Location> locations = new List<Location>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Lo_EditLocation";
                Location oldLoc = Get(claims, location.Id);

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = location.Id });

                if(location.City != oldLoc.City)
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
                        if(rowsAffected == 1)
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
            }

            return false;
        }

        /// <summary>
        /// Deletes a location entity from the database
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="id">The identifier of the dataset to be deleted</param>
        /// <returns>true if the entity was successfully deleted, false if not</returns>
        public static bool Delete(JwtClaims claims, long id)
        {
            if (PermissionService.IsPermitted("deleteLocation", claims.Role))
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
            }

            return false;
        }
    }
}
