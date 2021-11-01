using System;

namespace StudyBuddy.Model
{
    public class GameBadgeFilter
    {
        public int Start { get; set; } = 0;
        public int Count { get; set; } = 1000;
        public int? OwnerId { get; set; } = null;
        public string SearchText { get; set; }
    }
}
