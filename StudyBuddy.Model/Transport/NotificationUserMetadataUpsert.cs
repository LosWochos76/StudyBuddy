namespace StudyBuddy.Model
{
    public class NotificationUserMetadataUpsert
    {
        public int NotificationId { get; set; }
        public int OwnerId { get; set; }
        public bool? Liked { get; set; }
        public bool? Seen { get; set; }
        public bool? Shared { get; set; }
    }
}