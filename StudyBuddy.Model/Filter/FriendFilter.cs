namespace StudyBuddy.Model
{
    public class FriendFilter : BaseFilter
    {
        public int UserId { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public bool WithFriendshipRequest { get; set; }
        public bool OnlyActiveAccounts { get; set; } = true;
        public bool OnlyConfirmedAccounts { get; set; } = true;
    }
}