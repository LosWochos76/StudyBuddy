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
        private ChallengeListViewModel ChallengeList { get; set; }
        public IAsyncCommand RefreshCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        
        public AcceptedChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) 
            : base(api, dialog, navigation)
        {
            Challenges = new RangeObservableCollection<ChallengeViewModel>();
            ChallengeList = new ChallengeListViewModel();
            RefreshCommand = new AsyncCommand(RefreshView);
            Refresh();
        }

        private async Task RefreshView()
        {
            Refresh();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        public async void Refresh()
        {
            try
            {
                ChallengeList = await api.Challenges.GetAcceptedChallenges();
                if(ChallengeList != null)
                {
                    Challenges.Clear();
                    Challenges.AddRange(ChallengeList.Objects);
                }
            }
            catch (System.Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", $"Fehler beim Laden der Statistiken.{e}", "Ok");
            }
        }
    }
}
