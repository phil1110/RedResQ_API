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

            return $"{hazard.Timestamp.ToString("yyyy MM d").Replace(' ', '_')}_{hazard.Id}_{hazard.Title.Substring(0, 8).Trim().Replace(" ", "")}";
        }

        public static string GetHazardTopic(Hazard hazard)
        {
            return $"{hazard.Timestamp.ToString("yyyy MM d").Replace(' ', '_')}_{hazard.Id}_{hazard.Title.Substring(0, 8).Trim().Replace(" ", "")}";
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
