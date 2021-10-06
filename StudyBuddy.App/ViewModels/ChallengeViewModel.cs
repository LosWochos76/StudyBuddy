using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengeViewModel
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
        public int? SeriesParentID { get; set; }
        public string Tags { get; set; }

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

        public string PointsText
        {
            get
            {
                return Points > 1 ? Points + " Punkte" : Points + " Punkt";
            }
        }

        public string CategorySymbol
        {
            get
            {
                if (Category == ChallengeCategory.Learning)
                    return FontAwesomeIcons.GraduationCap;

                if (Category == ChallengeCategory.Networking)
                    return FontAwesomeIcons.PeopleArrows;

                return FontAwesomeIcons.Tasks;
            }
        }

        public static ChallengeViewModel FromModel(Challenge c)
        {
            return new ChallengeViewModel()
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                Points = c.Points,
                ValidityStart = c.ValidityStart,
                ValidityEnd = c.ValidityEnd,
                Category = c.Category,
                OwnerID = c.OwnerID,
                Created = c.Created,
                Prove = c.Prove,
                SeriesParentID = c.SeriesParentID
            };
        }

        private static IEnumerable<string> SplitIntoKeywords(string search_text)
        {
            return search_text.ToLower().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static bool ContainsAny(string property, IEnumerable<string> keywords)
        {
            return property == null ? false : keywords.Any(kw => property.ToLower().Contains(kw));
        }

        public bool ContainsAny(string search_text)
        {
            var keywords = SplitIntoKeywords(search_text);
            return ContainsAny(Name + Description + Tags, keywords);
        }
    }
}