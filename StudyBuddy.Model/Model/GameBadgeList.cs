using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class GameBadgeList
    {
        public int Count { get; set; }
        public IEnumerable<GameBadge> Objects { get; set; }
    }
}
