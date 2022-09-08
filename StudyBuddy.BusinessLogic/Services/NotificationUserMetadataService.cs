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

        public void Upsert(NotificationUserMetadataUpsert upsert)
        {
            upsert.OwnerId = backend.CurrentUser.ID;
            var before = backend.Repository.NotificationUserMetadataRepository.FindNotificationUserMetadata(upsert);

            if (before != null)
            {
                if (before.Liked == false && upsert.Liked == true)
                {
                    var notification = backend.NotificationService.ById(upsert.NotificationId);
                    backend.PushNotificationService.SendUserLikedNotification(notification.OwnerId);
                }
            }

            backend.Repository.NotificationUserMetadataRepository.Upsert(upsert);
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