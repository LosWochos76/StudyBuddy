using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    public interface ICommentsRepository
    {
        IEnumerable<Comment> All(CommentFilter filter);
        void Insert(CommentInsert insert);
        void DeleteAllForNotification(int notification_id);
    }
}