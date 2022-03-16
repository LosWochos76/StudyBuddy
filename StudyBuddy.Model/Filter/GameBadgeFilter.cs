namespace StudyBuddy.Model
{
    public class GameBadgeFilter : BaseFilter
    {
        public int? OwnerId { get; set; } = null;
        public string SearchText { get; set; }
        public bool WithOwner { get; set; } = false;

        // if we only want to see accepted/unacepted badges, we need to pass the user-id as well
        public bool OnlyUnreceived { get; set; } = false;
        public bool OnlyReceived { get; set; } = false;
        public int CurrentUserId { get; set; } = 0;
    }
}