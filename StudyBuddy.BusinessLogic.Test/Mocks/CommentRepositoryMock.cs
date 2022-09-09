using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class CommentRepositoryMock : ICommentsRepository
    {
        private List<Comment> objects = new List<Comment>();

        public IEnumerable<Comment> All(CommentFilter filter)
        {
            return objects.Skip(filter.Start).Take(filter.Count);
        }

        public void DeleteAllForNotification(int notification_id)
        {
            this.objects.RemoveAll(obj => obj.NotificationId == notification_id);
        }

        public Comment Insert(Comment obj)
        {
            if (obj.Id == 0)
                obj.Id = objects.Count + 1;

            objects.Add(obj);
            return obj;
        }
    }
}

