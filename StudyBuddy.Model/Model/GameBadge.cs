using System;

namespace StudyBuddy.Model
{
    public class GameBadge : Entity
    {
        public int OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public string Name { get; set; }
        public double RequiredCoverage { get; set; } = 0.5;
        public string Description { get; set; }
        public string Tags { get; set; }
        public string IconKey { get; set; }
        public DateTime Received { get; set; }

        // Only filled in certain contexts
        public User Owner { get; set; }
    }
}