using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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
        public string Tags { get; set; }

        [JsonIgnore]
        public string ValidityText
        {
            get
            {
                var result = "";

                if (ValidityStart.Year == ValidityEnd.Year && ValidityStart.Month == ValidityEnd.Month)
                    result = ValidityStart.ToString("dd.") + "-" + ValidityEnd.ToString("dd.MM.yyyy");
                else if (ValidityStart.Date == ValidityEnd.Date)
                    result = ValidityEnd.ToString("dd.MM.yyyy");
                else
                    result = ValidityStart.ToString("dd.MM.yyyy") + "-" + ValidityEnd.ToString("dd.MM.yyyy");

                return result;
            }
        }

        [JsonIgnore]
        public string PointsText
        {
            get
            {
                return Points > 1 ? Points + " Punkte" : Points + " Punkt";
            }
        }

        [JsonIgnore]
        public char CategorySymbol
        {
            get
            {
                // GraduationCap
                if (Category == ChallengeCategory.Learning)
                    return '\uf19d';

                // PeopleArrows
                if (Category == ChallengeCategory.Networking)
                    return '\ue068';

                return '\uf0ae';
            }
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

        private IEnumerable<string> SplitSearchText(string search_text)
        {
            return search_text.ToLower().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private bool Contains(string property, IEnumerable<string> keywords)
        {
            return property == null ? false : keywords.All(kw => property.ToLower().Contains(kw));
        }

        public bool Contains(string search_text)
        {
            var keywords = SplitSearchText(search_text);
            return Contains(Name + Description + Tags, keywords);
        }
    }
}