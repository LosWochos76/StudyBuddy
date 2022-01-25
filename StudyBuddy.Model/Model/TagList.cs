using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class TagList
    {
        public int Count { get; set; }
        public IEnumerable<Tag> Objects { get; set; }
    }
}
