using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public class Challenge
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Points { get; set; } = 1;
        public bool IsActive { get; set; } = false;
        public ChallengeCategory Category { get; set; } = ChallengeCategory.Learning;
        public int OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public ChallengeProve Prove { get; set; } = ChallengeProve.ByTrust;
        public string TargetAudience { get; set; }
    }
}