using System;

namespace StudyBuddy.Model
{
    public class GameBadge
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public string Name { get; set; }
        public double RequiredCoverage { get; set; } = 0.5;
    }
}