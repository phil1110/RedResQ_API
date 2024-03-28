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
    public static class HazardTypeService
    {
        public static HazardType[] Fetch()
        {
            List<HazardType> hazardTypes = new List<HazardType>();
            string storedProcedure = "SP_Ht_GetHazardTypes";

            DataTable hazardTypeTable = SqlHandler.ExecuteQuery(storedProcedure);

            if (hazardTypeTable.Rows.Count > 0)
            {
                foreach (DataRow row in hazardTypeTable.Rows)
                {
                    hazardTypes.Add(Converter.ToHazardType(row.ItemArray.ToList()!));
                }

                return hazardTypes.ToArray();
            }

            throw new NotFoundException("No Hazard Types were found!");
        }

        public static bool Add(string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ht_AddHazardType";

            parameters.Add(new SqlParameter { ParameterName = "@name", SqlDbType = SqlDbType.VarChar, Value = name });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Edit(HazardType hazardType)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ht_EditHazardType";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = hazardType.Id });
            parameters.Add(new SqlParameter { ParameterName = "@name", SqlDbType = SqlDbType.VarChar, Value = hazardType.Name });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Delete(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ht_DeleteHazardType";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = id });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }
    }
}
