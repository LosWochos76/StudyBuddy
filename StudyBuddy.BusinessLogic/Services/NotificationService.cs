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

        public IEnumerable<Notification> GetNotificationFromUser(int user_id)
        {
            var filter = new NotificationFilter { UserID = user_id };
            return backend.Repository.Notifications.GetAll(filter);
        }

        public NotificationList GetAll(NotificationFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && filter.UserID.HasValue && filter.UserID!=backend.CurrentUser.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (filter == null)
                filter = new NotificationFilter();

            return new NotificationList()
            {
                Count = backend.Repository.Notifications.GetCount(filter),
                Objects = backend.Repository.Notifications.GetAll(filter)
            };
        }

        public IEnumerable<Notification> GetNotificationsForFriends(NotificationFilter filter)
        {
            if (filter == null || !filter.UserID.HasValue)
                throw new Exception("Missing parameters!");

            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != filter.UserID.Value)
                throw new Exception("Unauthorized!");

            var notifications = backend.Repository.Notifications.GetNotificationsForFriends(filter);
            foreach (var obj in notifications)
            {
                if (filter.WithOwner)
                    obj.Owner = backend.Repository.Users.ById(obj.OwnerId);

                if (filter.WithLikedUsers)
                    obj.LikedUsers = backend.Repository.Users.GetAllLikersForNotification(obj.Id);

                if (filter.WithComments)
                {
                    var cf = new CommentFilter { NotificationId = obj.Id };
                    obj.Comments = backend.Repository.CommentsRepository.All(cf);
                }

                if (obj.BadgeId.HasValue && filter.WithBadge)
                    obj.Badge = backend.Repository.GameBadges.ById(obj.BadgeId.Value);

                yield return obj;
            }
        }

        public void UserAcceptedChallenge(User user, Challenge challenge)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user.ID)
                throw new Exception("Unauthorized!");

            var notification = new Notification()
            {
                Title = "Herausforderung abgeschlossen",
                Body = String.Format("{0} {1} hat die Herausforderung '{2}' abgeschlossen.",
                    user.Firstname, user.Lastname, challenge.Name),
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
                Title = "Abzeichen erhalten",
                Body = String.Format("{0} {1} hat das Abzeichen '{2}' erhalten.",
                    user.Firstname, user.Lastname, badge.Name),
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