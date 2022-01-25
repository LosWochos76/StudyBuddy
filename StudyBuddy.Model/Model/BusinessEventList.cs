using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class BusinessEventList
    {
        public int Count { get; set; }
        public IEnumerable<BusinessEvent> Objects { get; set; }
    }
}
