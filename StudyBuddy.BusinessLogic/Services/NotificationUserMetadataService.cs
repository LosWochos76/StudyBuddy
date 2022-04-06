using System.Collections;
using System.Collections.ObjectModel;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic.Services
{
    public class NotificationUserMetadataService
    {
        private readonly IBackend _backend;
        public Collection<NotificationUserMetadata> Likes = new();

        public NotificationUserMetadataService(IBackend backend)
        {
            _backend = backend;
        }

        public void Upsert(NotificationUserMetadataUpsert upsert)
        {
            upsert.OwnerId = _backend.CurrentUser.ID;
            _backend.Repository.NotificationUserMetadataRepository.Upsert(upsert);
        }

        public IEnumerable GetAll(NotificationUserMetadataFilter filter)
        {
            var response = _backend.Repository.NotificationUserMetadataRepository.All();
            return response;
        }
    }
}