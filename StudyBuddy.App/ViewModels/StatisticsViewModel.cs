using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model.Model;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private UserStatistics _userStatistic;
        public UserStatistics UserStatistics
        {
            get { return _userStatistic; }
            set
            {
                _userStatistic = value;
                NotifyPropertyChanged("UserStatistics");
            }
        }

        public string ThisMonthTrendGlyph { get; set; }
        public Color ThisMonthTrendColor { get; set; }

        public string ThisWeekTrendGlyph { get; set; }
        public Color ThisWeekTrendColor { get; set;}

        public IAsyncCommand RefreshCommand { get; }
        public bool IsRefreshing { get; set; } = false;

        public ICommand RanksCommand { get; set; }
        public ICommand AcceptedChallengesDetailsCommand { get; set; }
        public ICommand ShowHistoryCommand { get; set; }


        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.RanksCommand = new Command(ShowRanks);
            this.AcceptedChallengesDetailsCommand = new Command(ShowAcceptedChallengeHistory);
            this.ShowHistoryCommand = new Command(ShowChallengeHistory);
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            RefreshCommand = new AsyncCommand(RefreshView);
        }

        public void ShowAcceptedChallengeHistory()
        {
            Navigation.Push(new AcceptedChallengesPage());
        }

        public void ShowChallengeHistory()
        {
            Navigation.Push(new ChallengeHistoryStatisticsPage());
        }

        private void ShowRanks()
        {
            Navigation.Push(new ChallengeHistoryStatisticsPage());
            //Navigation.Push(new RankingPage(UserStatistics.FriendsRank));
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {

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
                UserStatistics = await api.Statistics.GetUserStatistics();
            }
            catch (System.Exception)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", "Fehler beim Laden der Statistiken. API Endpunkt nicht erreichbar", "Ok");
            }
        }

        private Command showHistoryCommad;
        public ICommand ShowHistoryCommad => showHistoryCommad ??= new Command(PerformShowHistoryCommad);

        private void PerformShowHistoryCommad()
        {
        }
    }
}