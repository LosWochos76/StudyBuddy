using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class Team
    {
        public int ID { get; set; }

        [Required]
        [MinLength(5)]
        public string Name { get; set; }
    }
}