using System.Collections.Generic;
using System.Collections.ObjectModel;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic.Services
{
    public class NotificationUserMetadataService
    {
        private readonly IBackend _backend;

        public NotificationUserMetadataService(IBackend backend)
        {
            _backend = backend;
        }

        public void Upsert(NotificationUserMetadataUpsert upsert)
        {
            upsert.OwnerId = _backend.CurrentUser.ID;

            var before = _backend.Repository.NotificationUserMetadataRepository.FindNotificationUserMetadata(upsert);

            if (before != null)
            {
                if (before.Liked == false && upsert.Liked == true)
                {
                    var notification = _backend.NotificationService.ById(upsert.NotificationId);
                    _backend.PushNotificationService.SendUserLikedNotification(notification.OwnerId);
                }
            }
        
            
            _backend.Repository.NotificationUserMetadataRepository.Upsert(upsert);
        }

        public IEnumerable<NotificationUserMetadata> GetAll()
        {
            var response = _backend.Repository.NotificationUserMetadataRepository.GetAll();
            return response;
        }

        public IEnumerable<NotificationUserMetadata> GetAllUnseen()
        {
            var response = _backend.Repository.NotificationUserMetadataRepository.GetAllUnseen();
            return response;
        }
        
     
    }
}