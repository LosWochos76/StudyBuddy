using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.CommunityToolkit.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengesCompletedViewModel : ViewModelBase
    {
        public RangeObservableCollection<ChallengeViewModel> Challenges { get; private set; }
        public ChallengeViewModel SelectedChallenge { get; set; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand LoadMoreCommand { get; }
        public IAsyncCommand SearchCommand { get; }
        public IAsyncCommand DetailsCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public int Skip { get; set; }
        
        private string _searchText = string.Empty;
        public string SearchText 
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value)
                    return;

                _searchText = value ?? string.Empty;
                Task.Run(async () =>
                {
                    string searchText = _searchText;
                    await Task.Delay(1000);

                    if (_searchText == searchText)
                        await Refresh();
                });
            }
        }
        
        private int _itemTreshold = 1;
        public int ItemThreshold { get { return _itemTreshold; } set { _itemTreshold = value; NotifyPropertyChanged(); } }
        
        private bool _isBusy = false;
        public bool IsBusy { get { return _isBusy; } set { _isBusy = value; NotifyPropertyChanged(); } }
        
        
        public ChallengesCompletedViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Challenges = new RangeObservableCollection<ChallengeViewModel>();
            LoadMoreCommand = new AsyncCommand(LoadChallenges);
            SearchCommand = new AsyncCommand(Refresh);
            RefreshCommand = new AsyncCommand(Refresh);
            DetailsCommand = new AsyncCommand(ShowDetails);

            api.ChallengeAccepted += async (sender, e) => { await LoadChallenges(); };
        }
        private async Task ShowDetails()
        {
            if (SelectedChallenge == null)
                return;
            
            await Navigation.Push(new ChallengeCompletedDetailsPage(SelectedChallenge));
            SelectedChallenge = null;
            NotifyPropertyChanged(nameof(SelectedChallenge));
        }

        public async Task Refresh()
        {
            Challenges.Clear();
            Skip = 0;
            await LoadChallenges();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }
        private async Task LoadChallenges()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var challenges = await api.Challenges.Accepted(SearchText, Skip);
                if (challenges.Objects.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Challenges.AddRange(challenges.Objects);
                Skip += 10;
            }
            catch (ApiException e)
            {
                dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
