using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class BadgesViewModel : ViewModelBase
    {
        public IEnumerable<GameBadge> Badges { get; set; } = new List<GameBadge>();

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
