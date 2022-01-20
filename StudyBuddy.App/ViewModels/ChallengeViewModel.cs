using System;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using Xamarin.Forms;

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
        public string PointsTextZero
        {
            get => Points.ToString("D3") + "/100";
        }
        public string ValidityEndText
        {
            get => ValidityEnd.ToString("dd.MM.yyyy");
        }
        public string ValidityStartText
        {
            get => ValidityStart.ToString("dd.MM.yyyy");
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
        public string CategoryText
        {
            get => "(" + Category.ToString() + ")";
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

        public Color CategoryColor
        {
            get
            {
                switch (Category)
                {
                    case ChallengeCategory.Learning: return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#007AFF") : Color.FromHex("#0A84FF");
                    case ChallengeCategory.Networking: return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#5856D6") : Color.FromHex("#5E5CE6");
                    case ChallengeCategory.Organizing: return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#5AC8FA") : Color.FromHex("#64D2FF");
                    default: return Color.Black;
                }
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
                    case ChallengeProve.ByFriend: return "Durch Bestätigung eines/einer Freundes/Freundin";
                    case ChallengeProve.ByLocation: return "Durch geographische Position";
                    case ChallengeProve.ByKeyword: return "Durch Schlüsselwort";
                    case ChallengeProve.BySystem: return "Durch Systembeweis";
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