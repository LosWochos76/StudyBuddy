using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic.Services
{
    class CommentService : ICommentService
    {
        private readonly IBackend backend;

        public CommentService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<Comment> GetAll(CommentFilter filter)
        {
            return backend.Repository.CommentsRepository.All(filter);
        }

        public Comment Insert(Comment obj)
        {
            if (obj is null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && obj.OwnerId != backend.CurrentUser.ID)
                throw new Exception("Unauthorized!");

            var notification = backend.NotificationService.ById(obj.NotificationId);
            backend.PushNotificationService.SendUserCommentNotification(notification.OwnerId);
            return backend.Repository.CommentsRepository.Insert(obj);
        }
    }
}