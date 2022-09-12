using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class NotificationUserMetadataList
    {
        public int Count { get; set; }
        public IEnumerable<NotificationUserMetadata> Objects { get; set; }
    }
}