
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class GameBadge
    {
        public int ID { get; set; }
        public int OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;

        [Required]
        public string Name { get; set; }
        public double RequiredCoverage { get; set; } = 0.5;
    }
}