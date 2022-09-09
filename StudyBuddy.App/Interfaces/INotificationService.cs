using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.App.Api
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationViewModel>> GetNotificationsForFriends(int skip);
        Task<NotificationUserMetadata> Like(NotificationViewModel news);
        Task<NotificationUserMetadata> HasSeen(NotificationViewModel news);
        Task<CommentViewModel> AddComment(NotificationViewModel notification, string comment_text);
        Task<IEnumerable<CommentViewModel>> GetAllCommentsForNotification(CommentFilter filter);
    }
}