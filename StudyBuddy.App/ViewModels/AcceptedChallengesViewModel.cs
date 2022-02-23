using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StudyBuddy.App.Api;
using System.Text;
using StudyBuddy.App.Misc;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Threading.Tasks;
using System.Drawing;

namespace StudyBuddy.App.ViewModels
{
    public class AcceptedChallengesViewModel : ViewModelBase
    {
        public RangeObservableCollection<ChallengeViewModel> Challenges { get; private set; }
        public IAsyncCommand RefreshCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public int Skip { get; set; }
        private bool is_busy = false;
        public bool IsBusy
        {
            get { return is_busy; }
            set
            {
                is_busy = value;
                NotifyPropertyChanged();
            }
        }
        public AcceptedChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Challenges = new RangeObservableCollection<ChallengeViewModel>();
            Refresh();
        }

        public async void Refresh()
        {
            try
            {
                Challenges = await api.Challenges.GetAcceptedChallenges();
            }
            catch (System.Exception)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", "Fehler beim Laden der Statistiken. API Endpunkt nicht erreichbar", "Ok");
            }
        }

        
    }
}
