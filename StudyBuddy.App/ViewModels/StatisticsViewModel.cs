using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog,
            navigation)
        {
        }
    }
}