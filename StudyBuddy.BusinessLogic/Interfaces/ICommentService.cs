using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic
{
    public interface ICommentService
    {
        Comment Insert(Comment insert);
        IEnumerable<Comment> GetAll(CommentFilter filter);
    }
}