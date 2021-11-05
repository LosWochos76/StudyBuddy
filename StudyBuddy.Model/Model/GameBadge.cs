using System;

namespace StudyBuddy.Model
{
    public class GameBadge : Entity
    {
        public int? OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public string Name { get; set; }
        public double RequiredCoverage { get; set; } = 0.5;
        public string Description { get; set; }
        public string Tags { get; set; }

        // Da das Image wohl in der DB gespeichert werden soll, ist Path ja wohl falsch.
        // Sollte ja wohl eher ein Bitmap o.ä. sein
        // Zudem sollte das dann in einer eigenen Tabelle abgelegt werden.
        // Diese Tabelle braucht man dann ja später auch für das Profilild der User
        //public string ImagePath { get; set; }
    }
}