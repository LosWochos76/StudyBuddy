namespace StudyBuddy.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Badge
    {
        private const string ACTIVE_BADGE_PATH = "badge_icon.png";
        private const string PASSIVE_BADGE_PATH = "badge_icon_passive.png";

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double FinishedCriteria { get; set; }
        public bool Achieved { get; set; }
        public string ImagePath { get; set; }
        public double Progress { get; set; }
        public string DateAcquiredString { get; set; }
        public string StudentAchievedCount { get; set; }

        public Badge(int id, string name, string description, bool achieved, double progress)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Achieved = achieved;
            this.Progress = progress / 100;
            this.ImagePath = this.Achieved ? ACTIVE_BADGE_PATH : PASSIVE_BADGE_PATH;
            this.DateAcquiredString = this.Achieved ? "Erhalten am: 09.10.2021" : "";
            this.StudentAchievedCount = this.Achieved ? "2% der Studerenden haben dieses Badge erhalten" : "";

            if (id == 3)
            {
                this.ImagePath = "confetti.png";
            }
        }
    }
}
