using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class NotificationRepositoryMock : INotificationRepository
    {
        private List<Notification> objects = new List<Notification>();

        public NotificationRepositoryMock()
        {
        }

        public Notification ById(int id)
        {
            return objects.Where(obj => obj.Id == id).FirstOrDefault();
        }

        public void Delete(int id)
        {
            objects.RemoveAll(obj => obj.Id == id);
        }

        public IEnumerable<Notification> GetAll(NotificationFilter filter)
        {
            return objects;
        }

        public int GetCount(NotificationFilter filter)
        {
            return objects.Count;
        }

        public IEnumerable<Notification> GetNotificationsForFriends(NotificationFilter filter)
        {
            throw new NotImplementedException();
        }

        public void Insert(Notification obj)
        {
            if (obj.Id == 0)
                obj.Id = GetCount(null) + 1;

            objects.Add(obj);
        }
    }
}