using System;
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
            if (backend.CurrentUser is null)
                throw new Exception("Unauthorized!");

            if (obj is null)
                throw new Exception("Object is null!");

            obj.OwnerId = backend.CurrentUser.ID;
            var existing = backend.Repository.NotificationUserMetadataRepository.FindByNotificationAndOwner(obj.NotificationId, obj.OwnerId);
            if (existing is not null)
            {
                if (existing.Liked == false && obj.Liked == true)
                {
                    var notification = backend.NotificationService.ById(obj.NotificationId);
                    backend.PushNotificationService.SendUserLikedNotification(notification.OwnerId);
                }

                obj.Id = existing.Id;
                obj.Created = existing.Created;
                backend.Repository.NotificationUserMetadataRepository.Update(obj);
            }
            else
            {
                backend.Repository.NotificationUserMetadataRepository.Insert(obj);
            }
        }

        public IEnumerable<NotificationUserMetadata> GetAll()
        {
            var response = backend.Repository.NotificationUserMetadataRepository.GetAll();
            return response;
        }
    }
}