using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class UserList
    {
        public int Count { get; set; }
        public IEnumerable<User> Objects { get; set; }
    }
}
