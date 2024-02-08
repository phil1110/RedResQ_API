using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return sb.ToString();
        }

        public static async Task<bool> InitializeHazardTopic(long id)
        {
            Hazard hazard = HazardService.Get(id);

            string topicName = GetHazardTopic(hazard);

            string[] tokens = CoordinateService.GetTokens(hazard.Latitude, hazard.Longitude, hazard.Radius);

            await NotificationService.RegisterForTopic(topicName, tokens.ToList());

            return true;
        }
    }
}
