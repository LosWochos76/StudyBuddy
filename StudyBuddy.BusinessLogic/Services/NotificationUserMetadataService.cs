using System.Collections.Generic;
using System.Collections.ObjectModel;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic.Services
{
    public class NotificationUserMetadataService : INotificationUserMetadataService
    {
        private readonly IBackend backend;

        public NotificationUserMetadataService(IBackend backend)
        {
            this.backend = backend;
        }

        public void Upsert(NotificationUserMetadata obj)
        {
            obj.OwnerId = backend.CurrentUser.ID;
            var before = backend.Repository.NotificationUserMetadataRepository.FindNotificationUserMetadata(obj);

            if (before != null && before.Liked == false && obj.Liked == true)
            {
                var notification = backend.NotificationService.ById(obj.NotificationId);
                backend.PushNotificationService.SendUserLikedNotification(notification.OwnerId);
            }

            backend.Repository.NotificationUserMetadataRepository.Upsert(obj);
        }

        public IEnumerable<NotificationUserMetadata> GetAll()
        {
            var response = backend.Repository.NotificationUserMetadataRepository.GetAll();
            return response;
        }

        public IEnumerable<NotificationUserMetadata> GetAllUnseen()
        {
            var response = backend.Repository.NotificationUserMetadataRepository.GetAllUnseen();
            return response;
        }
    }
}