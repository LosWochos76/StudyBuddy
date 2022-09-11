using System;
using System.Collections.Generic;
using System.Linq;
using FirebaseAdmin.Messaging;
using StudyBuddy.Model;
using StudyBuddy.Model.Enum;
using Notification = FirebaseAdmin.Messaging.Notification;

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
            var filter = new FcmTokenFilter();
            var fcmTokens = backend.Repository.FcmTokens.GetAll(filter)
                .Select(token => token.Token)
                .ToList();

            var message = new MulticastMessage()
            {
                Tokens = fcmTokens,
                Notification = new Notification()
                {
                    Title = obj.Title,
                    Body = obj.Body
                }
            };

            try
            {
                FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            }
            catch (Exception e)
            {
                backend.Logging.LogError("Error sending push message: " + e.ToString());
            }
        }

        public async void SendMessage(IEnumerable<string> tokens, string title, string body, PushNotificationData pushNotificationData = null)
        {
            var message = new MulticastMessage
            {
                Tokens = tokens.ToList(),
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = pushNotificationData.toData()
            };

            try
            {
                await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            }
            catch (Exception e)
            {
                backend.Logging.LogError("Error sending push message: " + e.ToString());
            }
        }

        public void SendUserLikedNotification(int userId)
        {
            if (backend.CurrentUser.ID == userId) return;

            var user = backend.Repository.Users.ById(userId);
            var fcmTokens = backend.Repository.FcmTokens.GetForUser(user.ID).Select(token => token.Token);
            
            backend.PushNotificationService.SendMessage(fcmTokens, "Gameucation", $"{this.backend.CurrentUser.Firstname} {this.backend.CurrentUser.Lastname} gefällt Ihr Beitrag.", new PushNotificationData()
            {
                PushNotificationType = PushNotificationTypes.Liked
            });
        }

        public void SendUserCommentNotification(int userId)
        {
            if (backend.CurrentUser.ID == userId)
                return;

            var user = backend.Repository.Users.ById(userId);
            var fcmTokens = backend.Repository.FcmTokens.GetForUser(user.ID).Select(token => token.Token);

            backend.PushNotificationService.SendMessage(fcmTokens, "Gameucation",
            $"{this.backend.CurrentUser.Firstname} {this.backend.CurrentUser.Lastname} hat ihren Beitrag kommentiert.", new PushNotificationData()
            {
                PushNotificationType = PushNotificationTypes.Comment
            });
        }
    }
}