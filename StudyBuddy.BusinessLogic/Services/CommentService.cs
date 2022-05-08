using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic.Services
{
    public class CommentService
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

        public void CreateComment(CommentInsert insert)
        {


            var notification = backend.NotificationService.GetNotificationById(insert.NotificationId);
            backend.PushNotificationService.SendUserCommentNotification(notification.OwnerId);

            backend.Repository.CommentsRepository.Insert(new CommentInsert
            {
                OwnerId = backend.CurrentUser.ID,
                NotificationId = insert.NotificationId,
                Text = insert.Text
            });
        }


    }
}
