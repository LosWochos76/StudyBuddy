using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class RequestList
    {
        public int Count { get; set; }
        public IEnumerable<Request> Objects { get; set; }
    }
}