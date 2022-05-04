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
        public GameBadge Clone()
        {
            var clone = new GameBadge();
            clone.OwnerID = OwnerID;
            clone.Created = DateTime.Now.Date;
            clone.Name = Name;
            clone.RequiredCoverage = RequiredCoverage;
            clone.Description = Description;
            clone.Tags = Tags;
            clone.Owner = Owner;
            return clone;
        }
        // Only filled in certain contexts
        public User Owner { get; set; }
    }
}