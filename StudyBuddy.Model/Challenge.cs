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
        public DateTime ValidityStart { get; set; } = DateTime.Now.Date;
        public DateTime ValidityEnd { get; set; } = DateTime.Now.Date;
        public ChallengeCategory Category { get; set; } = ChallengeCategory.Learning;
        public int OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public ChallengeProve Prove { get; set; } = ChallengeProve.ByTrust;
        public string TargetAudience { get; set; }
        public int? SeriesParentID { get; set; }

        public Challenge()
        {

        }

        public Challenge(int id, string name, string description, int points, int ownerId, string targetAudience)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Points = points;
            this.OwnerID = ownerId;
            this.TargetAudience = targetAudience;
        }

        public Challenge Clone()
        {
            var clone = new Challenge();
            clone.Name = Name;
            clone.Description = Description;
            clone.Points = Points;
            clone.ValidityStart = ValidityStart;
            clone.ValidityEnd = ValidityEnd;
            clone.Category = Category;
            clone.OwnerID = OwnerID;
            clone.Created = DateTime.Now.Date;
            clone.Prove = Prove;
            clone.TargetAudience = TargetAudience;
            clone.SeriesParentID = SeriesParentID;
            return clone;
        }
    }
}