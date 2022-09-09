using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class NotificationRepositoryMock : INotificationRepository
    {
        private UserRepositoryMock users;
        private List<Notification> objects = new List<Notification>();

        public NotificationRepositoryMock(UserRepositoryMock users)
        {
            this.users = users;
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
            var friend_filter = new FriendFilter() { UserId=filter.UserID.Value };
            var friends = this.users.GetFriends(friend_filter);

            foreach (var f in friends)
                foreach (var n in this.objects.FindAll(obj => obj.OwnerId == f.ID))
                    yield return n;
        }

        public void Insert(Notification obj)
        {
            if (obj.Id == 0)
                obj.Id = GetCount(null) + 1;

            objects.Add(obj);
        }
    }
}