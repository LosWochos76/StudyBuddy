using System;
using System.Collections.Generic;
using System.Linq;
using Google.Api.Gax;
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

        public Notification ById(int notificationId)
        {
            return backend.Repository.Notifications.ById(notificationId);
        }

        public void CreateNotificationForUser(int userId, string title, string body, int? badgeId = null)
        {
            var notification = new Notification
            {
                OwnerId = userId,
                Title = title,
                Body = body,
                BadgeId = badgeId
            };

            backend.Repository.Notifications.Insert(notification);
        }

        public IEnumerable<Notification> GetNotificationFromUser(int userId)
        {
            var response = backend.Repository.Notifications.GetAll(new NotificationFilter
            {
                OwnerId = userId
            });

            return response;
        }

        public NotificationList GetAll(NotificationFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && filter.OwnerId.HasValue && filter.OwnerId!=backend.CurrentUser.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (filter == null)
                filter = new NotificationFilter();

            return new NotificationList()
            {
                Count = backend.Repository.Notifications.GetCount(filter),
                Objects = backend.Repository.Notifications.GetAll(filter)
            };
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

        public void UserAcceptedChallenge(User user, Challenge challenge)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user.ID)
                throw new Exception("Unauthorized!");

            var notification = new Notification()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Title = "ChallengeAccepted",
                Body = String.Format("{0} {1} hat die Herausforderung '{2}' abgeschlossen.", user.Firstname, user.Lastname, challenge.Name),
                OwnerId = user.ID
            };

            backend.Repository.Notifications.Insert(notification);
        }

        public void UserReceivedBadge(User user, GameBadge badge)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user.ID)
                throw new Exception("Unauthorized!");

            var notification = new Notification()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Title = "BadgeReceived",
                Body = String.Format("{0} {1} hat das Abzeichen '{2}' erhalten.", user.Firstname, user.Lastname, badge.Name),
                OwnerId = user.ID
            };

            backend.Repository.Notifications.Insert(notification);
        }

        public void Delete(int notification_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.NotificationUserMetadataRepository.DeleteAllForNotification(notification_id);
            backend.Repository.CommentsRepository.DeleteAllForNotification(notification_id);
            backend.Repository.Notifications.Delete(notification_id);
        }
    }
}