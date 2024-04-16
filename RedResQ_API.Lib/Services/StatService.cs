using RedResQ_API.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public class StatService
    {
        public static Dictionary<string, int> GetStat(string statType)
        {
            string storedProcedure = "STAT_" + statType;

            DataTable statTable = SqlHandler.ExecuteQuery(storedProcedure);

            if (statTable.Rows.Count > 0)
            {
                Dictionary<string, int> stat = new Dictionary<string, int>();

                foreach (DataRow row in statTable.Rows)
                {
                    stat.Add(Convert.ToString(row.ItemArray[0])!, Convert.ToInt32(row.ItemArray[1])!);
                }

                return stat;
            }

            throw new NotFoundException("No Stats were found!");
        }
    }
}
