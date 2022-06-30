namespace StudyBuddy.Model
{
    public class GameBadgeFilter : BaseFilter
    {
        public int? OwnerId { get; set; } = null;
        public string SearchText { get; set; }
        public bool WithOwner { get; set; } = false;
    }
}