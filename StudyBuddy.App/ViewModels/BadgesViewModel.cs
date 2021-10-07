using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class BadgesViewModel : ViewModelBase
    {
        public IEnumerable<BadgeViewModel> Badges { get; set; } = new List<BadgeViewModel>();
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; set; }

        public BadgesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
        }

        public async void Reload()
        {
            Badges = await api.Badges.GetBadges();
            NotifyPropertyChanged("Badges");
        }
    }
}
