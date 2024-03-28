using RedResQ_API.Lib.Exceptions;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public class StatTypeService
    {
        public static string[] Fetch()
        {
            string storedProcedure = "SP_St_FetchTypes";

            DataTable statTypeTable = SqlHandler.ExecuteQuery(storedProcedure);

            if (statTypeTable.Rows.Count > 0)
            {
                List<string> statTypes = new List<string>();

                foreach (DataRow row in statTypeTable.Rows)
                {
                    statTypes.Add(Convert.ToString(row.ItemArray[0])!);
                }

                return statTypes.ToArray();
            }

            throw new NotFoundException("No Stat Types were found!");
        }
    }
}
