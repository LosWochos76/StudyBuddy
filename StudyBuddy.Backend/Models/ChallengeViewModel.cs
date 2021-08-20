using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyBuddy.Model
{
    public class ChallengeViewModel
    {
        public int ID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public DateTime ValidityStart { get; set; }

        [Required]
        public DateTime ValidityEnd { get; set; }

        [Required]
        public ChallengeCategory Category { get; set; }

        public int? SeriesParentID { get; set; }

        public SelectList AllCategories
        { 
            get 
            {
                return new SelectList(Enum.GetValues(typeof(ChallengeCategory)));
            }
        }

        [Required]
        public int OwnerID { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public ChallengeProve Prove { get; set; }

        public SelectList AllProves
        { 
            get 
            {
                return new SelectList(Enum.GetValues(typeof(ChallengeProve)));
            }
        }

        public string TargetAudience { get; set; }

        public static ChallengeViewModel FromChallenge(Challenge obj)
        {
            var challenge = new ChallengeViewModel();
            challenge.ID = obj.ID;
            challenge.Name = obj.Name;
            challenge.Description = obj.Description;
            challenge.Points = obj.Points;
            challenge.ValidityStart = obj.ValidityStart;
            challenge.ValidityEnd = obj.ValidityEnd;
            challenge.Category = obj.Category;
            challenge.OwnerID = obj.OwnerID;
            challenge.Created = obj.Created;
            challenge.Prove = obj.Prove;
            challenge.TargetAudience = obj.TargetAudience;
            challenge.SeriesParentID = obj.SeriesParentID;
            return challenge;
        }

        public static Challenge ToChallenge(ChallengeViewModel obj)
        {
            var challenge = new Challenge();
            challenge.ID = obj.ID;
            challenge.Name = obj.Name;
            challenge.Description = obj.Description;
            challenge.Points = obj.Points;
            challenge.ValidityStart = obj.ValidityStart;
            challenge.ValidityEnd = obj.ValidityEnd;
            challenge.Category = obj.Category;
            challenge.OwnerID = obj.OwnerID;
            challenge.Created = obj.Created;
            challenge.Prove = obj.Prove;
            challenge.TargetAudience = obj.TargetAudience;
            challenge.SeriesParentID = obj.SeriesParentID;
            return challenge;
        }
    }
}