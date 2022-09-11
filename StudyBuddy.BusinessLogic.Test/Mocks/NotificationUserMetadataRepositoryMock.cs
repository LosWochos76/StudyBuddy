using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Persistence;
using Xunit;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class NotificationUserMetadataRepositoryMock : INotificationUserMetadataRepository
    {
        private List<NotificationUserMetadata> objects = new List<NotificationUserMetadata>();

        public NotificationUserMetadataRepositoryMock()
        {
        }

        public IEnumerable<NotificationUserMetadata> AllForNotification(int notification_id)
        {
            return objects.Where(obj => obj.NotificationId == notification_id);
        }

        public void DeleteAllForNotification(int notification_id)
        {
            objects.RemoveAll(obj => obj.NotificationId == notification_id);
        }

        public NotificationUserMetadata FindByNotificationAndOwner(int notificationId, int ownerId)
        {
            return objects
                .Where(obj => obj.NotificationId == notificationId && obj.OwnerId == ownerId)
                .FirstOrDefault();
        }

        public IEnumerable<NotificationUserMetadata> GetAll()
        {
            return objects;
        }

        public void Insert(NotificationUserMetadata obj)
        {
            if (obj.Id == 0)
                obj.Id = this.objects.Count + 1;

            this.objects.Add(obj);
        }

        public void Update(NotificationUserMetadata obj)
        {
            int pos = objects.FindIndex(o => o.Id == obj.Id);
            objects[pos] = obj;
        }
    }
}