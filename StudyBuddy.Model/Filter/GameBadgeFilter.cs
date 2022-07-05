namespace StudyBuddy.Model
{
    public class GameBadgeFilter : BaseFilter
    {
        public int? OwnerId { get; set; } = null;
        public string SearchText { get; set; }
        public bool WithOwner { get; set; } = false;

        public GameBadgeFilter()
        {
        }

        public GameBadgeFilter(string search_text, int start)
        {
            if (search_text != string.Empty)
                SearchText = search_text;

            if (start != 0)
            {
                Start = start;
                Count = 10;
            }
        }
    }
}