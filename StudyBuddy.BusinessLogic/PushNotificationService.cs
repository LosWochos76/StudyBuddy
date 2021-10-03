using FirebaseAdmin.Messaging;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class PushNotificationService
    {
        private readonly IRepository repository;
        private readonly User current_user;

        public PushNotificationService(IRepository repository, User current_user)
        {
            this.repository = repository;
            this.current_user = current_user;
        }

        public void BroadcastMessage(string title, string body)
        {
            var fcmTokens = repository.FcmTokens.All();

            var message = new Message
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            foreach (var fcmToken in fcmTokens)
            {
                message.Token = fcmToken.Token;
                FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
        }
    }
}