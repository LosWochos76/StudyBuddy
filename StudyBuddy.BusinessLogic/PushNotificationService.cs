using FirebaseAdmin.Messaging;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class PushNotificationService
    {
        private IRepository repository;
        
        public PushNotificationService(IRepository repository)
        {
            this.repository = repository;
        }



        public void BroadcastMessage(string title, string body)
        {

            var fcmTokens = repository.FcmTokens.All();

            var message = new Message
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
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