using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

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
            var existing = backend.Repository.NotificationUserMetadata.FindByNotificationAndOwner(obj.NotificationId, obj.OwnerId);
            if (existing is not null)
            {
                if (existing.Liked == false && obj.Liked == true)
                {
                    var notification = backend.NotificationService.ById(obj.NotificationId);
                    backend.PushNotificationService.SendUserLikedNotification(notification.OwnerId);
                }

                obj.Id = existing.Id;
                obj.Created = existing.Created;
                backend.Repository.NotificationUserMetadata.Update(obj);
            }
            else
            {
                backend.Repository.NotificationUserMetadata.Insert(obj);
            }
        }

        public NotificationUserMetadataList GetAll(NotificationUserMetadataFilter filter)
        {
            if (backend.CurrentUser is null)
                throw new Exception("Unauthorized!");
            
            return new NotificationUserMetadataList()
            {
                Count = backend.Repository.NotificationUserMetadata.GetCount(filter),
                Objects = backend.Repository.NotificationUserMetadata.GetAll(filter)
            };
        }

        public void Delete(int id)
        {
            
            if (!backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");
            
            this.backend.Repository.NotificationUserMetadata.Delete(id);
        }

    }
}