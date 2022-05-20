using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengeViewModel : Challenge
    {
        public string PointsTextZero
        {
            get => Points.ToString("D3");
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
            get
            {
                string cat = Category.ToString();
                return cat switch
                {
                    "Learning" => "(Lernen)",
                    "Networking" => "(Netzwerken)",
                    _ => "(Organisieren)",
                };
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

        public Color CategoryColor
        {
            get
            {
                switch (Category)
                {
                    case ChallengeCategory.Learning: return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#007AFF") : Color.FromHex("#007AFF");
                    case ChallengeCategory.Networking: return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#5856D6") : Color.FromHex("#5856D6");
                    case ChallengeCategory.Organizing: return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#5AC8FA") : Color.FromHex("#5AC8FA");
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
    }
}