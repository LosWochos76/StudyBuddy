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

        public async void SendMessage(IEnumerable<string> tokens, string title, string body, PushNotificationData pushNotificationData)
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

        public void SendUserAcceptedChallenge(User user, Challenge challenge)
        {
            var friends = this.backend.UserService.GetAllFriends(new FriendFilter()
            {
                UserId = user.ID
            });

            var tokens = friends.Objects.SelectMany(friend =>
                    this.backend.Repository.FcmTokens.GetForUser(friend.ID).Select(token => token.Token)
            );
            
            string title = "Gameucation";
            string body = $"{user.Firstname} {user.Lastname} hat die Herausforderung {challenge.Name} abgeschlossen.";
            
            this.backend.PushNotificationService.SendMessage(tokens, title, body, new PushNotificationData()
            {
                PushNotificationType = PushNotificationTypes.ChallengeAccepted
            });
        }

        public void SendUserLikedNotification(int userId)
        {
            if (backend.CurrentUser.ID == userId) return;

            var user = backend.Repository.Users.ById(userId);
            var fcmTokens = backend.Repository.FcmTokens.GetForUser(user.ID).Select(token => token.Token);
            
            string title = "Gameucation";
            string body = $"{this.backend.CurrentUser.Firstname} {this.backend.CurrentUser.Lastname} gefällt Ihr Beitrag.";
            
            backend.PushNotificationService.SendMessage(fcmTokens, title, body, new PushNotificationData()
            {
                PushNotificationType = PushNotificationTypes.Liked
            });
        }
        
        public void SendFriendShipRequestNotification(int senderId, int recipentId)
        {

            var senderUser = backend.Repository.Users.ById(senderId);
            var fcmTokens = backend.Repository.FcmTokens.GetForUser(recipentId).Select(token => token.Token);
            
            string title = "Gameucation";
            string body = $"{senderUser.Firstname} {senderUser.Lastname} möchte Ihr Freund sein.";
            
            /*backend.PushNotificationService.SendMessage(fcmTokens, title, body, new PushNotificationData()
            {
                PushNotificationType = PushNotificationTypes.FriendShipRequest
            });*/
        }
        
        public void SendFriendShipAcceptedNotification(int senderId, int recipentId)
        {

            var recipentUser = backend.Repository.Users.ById(recipentId);
            var fcmTokens = backend.Repository.FcmTokens.GetForUser(senderId).Select(token => token.Token);
            
            string title = "Gameucation";
            string body = $"{recipentUser.Firstname} {recipentUser.Lastname} ist nun ihr Freund.";
            
            backend.PushNotificationService.SendMessage(fcmTokens, title, body, new PushNotificationData()
            {
                PushNotificationType = PushNotificationTypes.FriendShipAccepted
            });
        }
        
    }
}