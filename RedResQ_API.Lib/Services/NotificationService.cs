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
            // This registration token comes from the client FCM SDKs.
            var registrationToken = token;

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "title", title },
                    { "desc", desc },
                },
                Token = registrationToken,
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);

            return response;
        }
    }
}
