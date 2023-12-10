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
        public static Location Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_GetLocation";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable locationTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (locationTable.Rows.Count == 1)
            {
                return Location.ConvertToLocation(locationTable.Rows[0]);
            }

            throw new Exception("Row count was not 1!");
        }

        public static Location[] Fetch(long? id)
        {
            List<Location> locations = new List<Location>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Lo_GetLocations";

            if (id.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id.Value });
            }

            DataTable languageTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (languageTable.Rows.Count > 0)
            {
                foreach (DataRow row in languageTable.Rows)
                {
                    locations.Add(Location.ConvertToLocation(row));
                }

                return locations.ToArray();
            }

            throw new Exception("No Locations were found!");
        }
    }
}
