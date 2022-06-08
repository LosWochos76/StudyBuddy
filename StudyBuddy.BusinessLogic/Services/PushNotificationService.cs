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
            var fcmTokens = backend.Repository.FcmTokens.GetAll()
                .Select(token => token.Token)
                .ToList();

            FirebaseMessaging.DefaultInstance.SendMulticastAsync(new MulticastMessage
            {
                Tokens = fcmTokens,
                Notification =
                {
                    Title = obj.Title,
                    Body = obj.Body
                }
            });
        }

        public async void SendMessage(IEnumerable<string> tokens, string title, string body,
            PushNotificationData pushNotificationData = null)
        {
            try
            {
                await FirebaseMessaging.DefaultInstance.SendMulticastAsync(new MulticastMessage
                {
                    Tokens = tokens.ToList(),
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = pushNotificationData.toData()
                });
            }
            catch (
                FirebaseMessagingException e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public void SendNewNotificationsAvailable()
        {
            var metadatas = backend.NotificationUserMetadataService.GetAllUnseen().ToList();
            var users = backend.Repository.Users.All(new UserFilter()).ToList();

            users.ForEach(user =>
            {
                var userMetaDatas = metadatas.FindAll(userMetadata => userMetadata.OwnerId == user.ID);

                if (userMetaDatas.Count == 0) return;


                var text = $"Sie haben {userMetaDatas.Count} neue Benachrichtigungen.";

                var fcmTokens = backend.FcmTokenService.GetForUser(user.ID).Select(token => token.Token).ToList();
                backend.PushNotificationService.SendMessage(fcmTokens, "Gameucation", text,  new PushNotificationData()
                {
                    PushNotificationType = PushNotificationTypes.NewNotifications
                });
            });
        }

        public void SendUserLikedNotification(int userId)
        {
            if (backend.CurrentUser.ID == userId) return;

            var user = backend.Repository.Users.ById(userId);
            var fcmTokens = backend.Repository.FcmTokens.ForUser(user.ID).Select(token => token.Token);
            

            backend.PushNotificationService.SendMessage(fcmTokens, "Gameucation", $"{this.backend.CurrentUser.Firstname} {this.backend.CurrentUser.Lastname} gefällt Ihr Beitrag.", new PushNotificationData()
            {
                PushNotificationType = PushNotificationTypes.Liked
            });
        }

        public void SendUserCommentNotification(int userId)
        {
            if (backend.CurrentUser.ID == userId) return;

            var user = backend.Repository.Users.ById(userId);
            var fcmTokens = backend.Repository.FcmTokens.ForUser(user.ID).Select(token => token.Token);

            backend.PushNotificationService.SendMessage(fcmTokens, "Gameucation",
                $"{this.backend.CurrentUser.Firstname} {this.backend.CurrentUser.Lastname} hat ihren Beitrag kommentiert.", new PushNotificationData()
                {
                    PushNotificationType = PushNotificationTypes.Comment
                });
        }
    }
}