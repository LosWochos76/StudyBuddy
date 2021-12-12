using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public class ChallengeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LearningChallengeTemplate { get; set; }
        public DataTemplate NetworkingChallengeTemplate { get; set; }
        public DataTemplate OrganizingChallengeTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((ChallengeViewModel)item).Category == ChallengeCategory.Learning)
                return LearningChallengeTemplate;
            if (((ChallengeViewModel)item).Category == ChallengeCategory.Networking)
                return NetworkingChallengeTemplate;
            if (((ChallengeViewModel)item).Category == ChallengeCategory.Organizing)
                return OrganizingChallengeTemplate;
            else
                return LearningChallengeTemplate;
        }
    }
}
