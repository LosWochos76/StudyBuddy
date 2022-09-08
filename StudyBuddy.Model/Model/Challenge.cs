using System;

namespace StudyBuddy.Model
{
    public class Challenge : Entity
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int Points { get; set; } = 1;
        public DateTime ValidityStart { get; set; } = DateTime.Now.Date;
        public DateTime ValidityEnd { get; set; } = DateTime.Now.Date;
        public ChallengeCategory Category { get; set; } = ChallengeCategory.Learning;
        public int OwnerID { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public ChallengeProve Prove { get; set; } = ChallengeProve.ByTrust;
        public string ProveAddendum { get; set; } = String.Empty;
        public int SeriesParentID { get; set; } = 0;
        public string Tags { get; set; } = String.Empty;

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
            clone.ProveAddendum = ProveAddendum;
            clone.SeriesParentID = SeriesParentID;
            return clone;
        }

        // Only filled in certain situations
        public User Owner { get; set; } = null;
    }
}