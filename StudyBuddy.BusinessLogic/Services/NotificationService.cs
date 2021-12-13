using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public class NotificationService : INotificationService
    {
        private readonly IBackend backend;

        public NotificationService(IBackend backend)
        {
            this.backend = backend;
        }

        public void CreateNotificationForUser(int userId, string title, string body)
        {
            backend.Repository.Notifications.Insert(new Notification()
            {
                OwnerId = userId,
                Title = title,
                Body = body
            });
        }

        public IEnumerable<Notification> GetNotificationFromUser(int userId)
        {
            var response = backend.Repository.Notifications.All(new NotificationFilter()
            {
                OwnerId = userId
            });

            return response;
        }

        public IEnumerable<Notification> GetNotificationFeedForUser(int userId)
        {
            return backend.Repository.Notifications.GetUserNotificationsFeed(new NotificationFilter()
            {
                OwnerId = userId
            });
        }
    }
}