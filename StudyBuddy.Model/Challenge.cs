using System;

namespace StudyBuddy.Model
{
    public class Challenge : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Points { get; set; } = 1;
        public DateTime ValidityStart { get; set; } = DateTime.Now.Date;
        public DateTime ValidityEnd { get; set; } = DateTime.Now.Date;
        public ChallengeCategory Category { get; set; } = ChallengeCategory.Learning;
        public int OwnerID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now.Date;
        public ChallengeProve Prove { get; set; } = ChallengeProve.ByTrust;
        public int? SeriesParentID { get; set; }

        public Challenge()
        {
        }

        public Challenge(int id, string name, string description, int points, int ownerId)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Points = points;
            this.OwnerID = ownerId;
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
            clone.SeriesParentID = SeriesParentID;
            return clone;
        }
    }
}