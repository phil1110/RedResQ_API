using RedResQ_API.Lib.Models;
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
    public static class TopicService
    {
        public static string GetHazardTopic(long id)
        {
            var hazard = HazardService.Get(id);

            return GetHazardTopic(hazard);
        }

        public static string GetHazardTopic(Hazard hazard)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(hazard.Timestamp.ToString("yyyy MM d").Replace(' ', '_'));
            sb.Append('_');
            sb.Append(hazard.Title.Length > 8 ? hazard.Title.Substring(0, 8).Trim().Replace(" ", "") : hazard.Title.Trim().Replace(" ", ""));
            sb.Append('_');
            sb.Append("ID" + hazard.Id);

            return sb.ToString();
        }

        public static async Task<bool> InitializeHazardTopic(long id)
        {
            Hazard hazard = HazardService.Get(id);

            string topicName = GetHazardTopic(hazard);

            string[] tokens = CoordinateService.GetTokens(hazard.Latitude, hazard.Longitude, hazard.Radius);

            var topicLogged = LogTopic(hazard.Id, topicName);

            Console.WriteLine("Topic logged: " + topicLogged);

            try
            {
                await NotificationService.RegisterForTopic(topicName, tokens.ToList());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool LogTopic(long hazardId, string topicName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Hc_AddTopic";

            parameters.Add(new SqlParameter { ParameterName = "@hazardId", SqlDbType = SqlDbType.BigInt, Value = hazardId });
            parameters.Add(new SqlParameter { ParameterName = "@topicName", SqlDbType = SqlDbType.VarChar, Value = topicName });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }

        public static string[] GetTopics(double latitude, double longitude)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Hc_GetTopics";

            parameters.Add(new SqlParameter { ParameterName = "@lat", SqlDbType = SqlDbType.Float, Value = latitude });
            parameters.Add(new SqlParameter { ParameterName = "@lon", SqlDbType = SqlDbType.Float, Value = longitude });

            var topicTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (topicTable.Rows.Count > 0)
            {
                List<string> topics = new List<string>();

                foreach (DataRow topic in topicTable.Rows)
                {
                    topics.Add(Convert.ToString(topic.ItemArray[0])!);
                }

                return topics.ToArray();
            }

            throw new NotFoundException();
        }
    }
}
