using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class Team
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<User> Members { get; set; }
    }
}