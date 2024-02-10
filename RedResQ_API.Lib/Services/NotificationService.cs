using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public static class NotificationService
    {
        static NotificationService()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(Constants.GoogleCredential),
            });
        }

        public async static Task<string> SendNotification(string token, string title, string desc)
        {
            var notification = new Notification()
            {
                Title = title,
                Body = desc
            };

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Notification = notification,
                //Data = new Dictionary<string, string>()
                //{
                //    { "title", title },
                //    { "desc", desc },
                //},
                Token = token,
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            Console.WriteLine("Successfully sent message: " + response);

            return response;
        }

        public static async Task<string> SendHazardNotification(long hazardId, string title, string desc)
        {
            var hazard = HazardService.Get(hazardId);
            var topic = TopicService.GetHazardTopic(hazard);

            var notification = new Notification()
            {
                Title = title,
                Body = desc
            };

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Notification = notification,
                //Data = new Dictionary<string, string>()
                //{
                //    { "score", "850" },
                //    { "time", "2:45" },
                //},
                Topic = topic
            };

            // Send a message to the devices subscribed to the provided topic.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);

            return response;
        }

        public static async Task RegisterForTopic(string topic, List<string> tokens)
        {
            // Subscribe the devices corresponding to the registration tokens to the
            // topic
            var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(tokens, topic);

            // See the TopicManagementResponse reference documentation
            // for the contents of response.
            Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully");
        }
    }
}
