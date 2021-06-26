using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class Challenge
    {
        public int ID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public ChallengeCategory Category { get; set; }

        [Required]
        public int OwnerID { get; set; }

        [Required]
        public ChallengeProve Prove { get; set; }

        [Required]
        public Term ValidFor { get; set; }
    }
}