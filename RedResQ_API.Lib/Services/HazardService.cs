using RedResQ_API.Lib.Exceptions;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedResQ_API.Lib.Services
{
    public static class HazardService
    {
        public static Hazard Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Hz_GetHazard";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable hazardTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (hazardTable.Rows.Count == 1)
            {
                return Converter.ToHazard(hazardTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException("No Hazard was found!");
        }

        public static Hazard[] Fetch(long? id, int? amount)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<Hazard> hazards = new List<Hazard>();
            string storedProcedure = "SP_Hz_FetchHazards";

            if (id.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });
            }
            if (amount.HasValue)
            {
                if (amount > 25)
                {
                    amount = 25;
                }

                parameters.Add(new SqlParameter { ParameterName = "@amount", SqlDbType = SqlDbType.Int, Value = amount });
            }

            DataTable hazardTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (hazardTable.Rows.Count > 0)
            {
                foreach (DataRow row in hazardTable.Rows)
                {
                    hazards.Add(Converter.ToHazard(row.ItemArray.ToList()!));
                }

                return hazards.ToArray();
            }

            throw new NotFoundException("No Hazards found!");
        }

        public static async Task<object> Add(string title, double lat, double lon, int radius, int typeId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Hz_AddHazard";

            parameters.Add(new SqlParameter { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = title });
            parameters.Add(new SqlParameter { ParameterName = "@lat", SqlDbType = SqlDbType.Float, Value = lat });
            parameters.Add(new SqlParameter { ParameterName = "@lon", SqlDbType = SqlDbType.Float, Value = lon });
            parameters.Add(new SqlParameter { ParameterName = "@radius", SqlDbType = SqlDbType.Int, Value = radius });
            parameters.Add(new SqlParameter { ParameterName = "@typeId", SqlDbType = SqlDbType.Int, Value = typeId });


            DataTable hazardTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (hazardTable.Rows.Count == 1)
            {
                long id = Convert.ToInt64(hazardTable.Rows[0].ItemArray[0]);

                bool hasInitialSubscribers = await TopicService.InitializeHazardTopic(id);

                return new { Id = id, HasInitialSubscribers = hasInitialSubscribers };
            }

            throw new UnprocessableEntityException();
        }

        public static bool Edit(long id, string? title, double? lat, double? lon, int? radius, int? typeId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Hz_EditHazard";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            if (title != null)
            {
                parameters.Add(new SqlParameter { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = title });
            }
            if (lat.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@lat", SqlDbType = SqlDbType.Float, Value = lat });
            }
            if (lon.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@lon", SqlDbType = SqlDbType.Float, Value = lon });
            }
            if (radius.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@radius", SqlDbType = SqlDbType.Int, Value = radius });
            }
            if (typeId.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@typeId", SqlDbType = SqlDbType.Int, Value = typeId });
            }

            if (parameters.Count > 1)
            {
                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected > 0)
                {
                    return true;
                }
            }

            throw new UnprocessableEntityException();
        }

        public static bool Delete(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Hz_DeleteHazard";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }
    }
}
