using System.Collections.Generic;
using FirebaseAdmin.Messaging;

namespace StudyBuddy.BusinessLogic
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IBackend backend;

        public PushNotificationService(IBackend backend)
        {
            this.backend = backend;
        }

        public void BroadcastMessage(PushNotificationBroadcastDto obj)
        {
            var fcmTokens = backend.Repository.FcmTokens.All();

            var message = new Message
            {
                Notification = new Notification
                {
                    Title = obj.Title,
                    Body = obj.Body,
                    
                },
            };

            foreach (var fcmToken in fcmTokens)
            {
                message.Token = fcmToken.Token;
                FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
        }

        public async void SendMessage(string token, string title, string body, PushNotificationData pushNotificationData = null)
        {

            var data = new Dictionary<string, string>();
            data.Add("data", pushNotificationData.toJson());
            var message = new Message
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                    
                },
                Data = data
            };
            
            message.Token = token;
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
        
    }
}