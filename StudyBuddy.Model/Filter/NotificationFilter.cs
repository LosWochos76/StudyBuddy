namespace StudyBuddy.Model
{
    public class NotificationFilter : BaseFilter
    { 
        public int? UserID { get; set; } = null;
        public bool WithOwner { get; set; } = false;
        public bool WithLikedUsers { get; set; } = false;
        public bool WithBadge { get; set; } = false;
    }
}