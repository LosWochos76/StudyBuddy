namespace StudyBuddy.Model
{
    public class FriendFilter : BaseFilter
    {
        public int UserId { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public bool WithProfileImages { get; set; } = false;
    }
}