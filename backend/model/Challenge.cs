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
        public DateTime TargetDate { get; set; } = DateTime.Now.Date;
        public ChallengeCategory Category { get; set; } = ChallengeCategory.Learning;
        public int OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public ChallengeProve Prove { get; set; } = ChallengeProve.ByTrust;
        public string TargetAudience { get; set; }

        public Challenge Clone()
        {
            var clone = new Challenge();
            clone.Name = Name;
            clone.Description = Description;
            clone.Points = Points;
            clone.TargetDate = TargetDate;
            clone.Category = Category;
            clone.OwnerID = OwnerID;
            clone.Created = DateTime.Now.Date;
            clone.Prove = Prove;
            clone.TargetAudience = TargetAudience;
            return clone;
        }
    }
}