using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

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
            backend.Repository.Notifications.Insert(new Notification
            {
                OwnerId = userId,
                Title = title,
                Body = body
            });
        }


        public IEnumerable<Notification> GetNotificationFromUser(int userId)
        {
            var response = backend.Repository.Notifications.All(new NotificationFilter
            {
                OwnerId = userId
            });

            return response;
        }

        public IEnumerable<Notification> GetNotificationFeedForUser(int userId, NotificationFilter filter)
        {
            var notifications = backend.Repository.Notifications.GetUserNotificationsFeed(new NotificationFilter
            {
                OwnerId = userId,
                Start = filter.Start,
                Count = filter.Count
            });


            foreach (var notification in notifications)
            {
                notification.LikedUsers =
                    backend.Repository.Users.GetAllLikersForNotification(notification.Id).ToList();
                notification.Comments = backend.Repository.CommentsRepository.All(new CommentFilter
                {
                    NotificationId = notification.Id
                }).ToList();
            }


            return notifications;
        }
    }
}