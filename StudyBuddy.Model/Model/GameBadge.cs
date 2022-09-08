using System;

namespace StudyBuddy.Model
{
    public class GameBadge : Entity
    {
        public int OwnerID { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public string Name { get; set; } = string.Empty;
        public double RequiredCoverage { get; set; } = 0.5;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string IconKey { get; set; } = "fa-solid,f559,#bf8970";
        public DateTime Received { get; set; } = DateTime.Now.Date;

        // Only filled in certain contexts
        public User Owner { get; set; } = null;
    }
}