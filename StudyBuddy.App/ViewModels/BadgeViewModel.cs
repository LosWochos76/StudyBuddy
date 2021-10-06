using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddy.App.ViewModels
{
    public class BadgeViewModel
    {
        public int? OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public string Name { get; set; }
        public double RequiredCoverage { get; set; } = 0.5;
        public string Description { get; set; }
        public string ImagePath { get; set; }

    }
}
