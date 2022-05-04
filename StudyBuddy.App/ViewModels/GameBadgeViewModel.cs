using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class GameBadgeViewModel : GameBadge
    {
        public string OwnerIdText
        {
            get => OwnerID.ToString();
        }
        public string CreatedText
        {
            get => Created.ToString("dd.MM.yyyy");
        }
        public string NameText
        {
            get => Name.ToString();
        }
        public string RequiredCoverageText
        {
            get => RequiredCoverage.ToString();
        }
        public string DescriptionText
        {
            get => Description.ToString();
        }
        public string TagsText
        {
            get => Tags.ToString();
        }
        public string OwnerText
        {
            get => Owner.ToString();
        }
    }
}