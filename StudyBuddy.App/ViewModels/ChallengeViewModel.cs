using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.App.Misc;
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
        public string ProveAddendum { get; set; }

        public string ValidityEndText
        {
            get
            {
                return ValidityEnd.ToString("dd.MM.yyyy");
            }
        }

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

        public string ProveText
        {
            get
            {
                switch (Prove)
                {
                    case ChallengeProve.ByTrust: return "Durch Vertrauen";
                    case ChallengeProve.ByQrCode: return "Durch einen QR-Code";
                    case ChallengeProve.ByFriend: return "Durch Bestätigung eines Freundes";
                    case ChallengeProve.ByLocation: return "Durch geographische Position";
                    case ChallengeProve.ByKeyword: return "Durch Schlüsselwort";
                    default: return "";
                }
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
                SeriesParentID = c.SeriesParentID,
                Tags = c.Tags,
                ProveAddendum = c.ProveAddendum
            };
        }

        public bool ContainsAny(string search_text)
        {
            if (string.IsNullOrEmpty(search_text))
                return true;

            var keywords = Helper.SplitIntoKeywords(search_text);
            return Helper.ContainsAny(Name + Description + Tags, keywords);
        }
    }
}