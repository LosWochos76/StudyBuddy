using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using System.Threading.Tasks;
using System.Windows.Input;
using Microcharts;
using SkiaSharp;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private UserStatistics _userStatistic;
        public UserStatistics UserStatistics { get { return _userStatistic; } set { _userStatistic = value; NotifyPropertyChanged(); } }
        private BarChart _totalPointsChart;
        public BarChart TotalPointsChart { get { return _totalPointsChart; } set { _totalPointsChart = value; NotifyPropertyChanged(); } }
        private DonutChart _totalChallengesChart;
        public DonutChart TotalChallengesChart { get { return _totalChallengesChart; } set { _totalChallengesChart = value; NotifyPropertyChanged(); } }
        public IAsyncCommand RefreshCommand { get; }
        public bool IsRefreshing { get; set; }
        public ICommand TotalChallengesCommand { get; set; }

        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.TotalChallengesCommand = new Command(ShowCompletedChallenges);
            RefreshCommand = new AsyncCommand(RefreshView);
        }

        private void ShowCompletedChallenges()
        {
            Navigation.Push(new ChallengesCompletedPage());
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
                await Application.Current.MainPage.DisplayAlert("Fehler", "Fehler beim Laden der Statistiken. API Endpunkt nicht erreichbar", "Ok");
            }
            LoadCharts();
        }

        private void LoadCharts()
        {
            var pointsEntries = new[]
            {
                new ChartEntry(UserStatistics.TotalNetworkChallengesPoints)
                {
                    ValueLabel = UserStatistics.TotalNetworkChallengesPoints.ToString(),
                    Color = SKColor.Parse("#5856D6")
                },
                new ChartEntry(UserStatistics.TotalOrganizingChallengesPoints)
                {
                    ValueLabel = UserStatistics.TotalOrganizingChallengesPoints.ToString(),
                    Color = SKColor.Parse("#5AC8FA")
                },
                new ChartEntry(UserStatistics.TotalLearningChallengesPoints)
                {
                    ValueLabel = UserStatistics.TotalLearningChallengesPoints.ToString(),
                    Color = SKColor.Parse("#007AFF")
                },
            };
            var challengesEntries = new[]
            {
                new ChartEntry(UserStatistics.TotalNetworkChallengesCount)
                {
                    Color = SKColor.Parse("#5856D6")
                },
                new ChartEntry(UserStatistics.TotalOrganizingChallengesCount)
                {
                    Color = SKColor.Parse("#5AC8FA")
                },
                new ChartEntry(UserStatistics.TotalLearningChallengesCount)
                {
                    Color = SKColor.Parse("#007AFF")
                },
            };
            TotalPointsChart = new BarChart {Entries = pointsEntries, LabelTextSize = 0, BackgroundColor = SKColor.Parse("#F2F2F7") ,ValueLabelOrientation = Orientation.Horizontal, ValueLabelTextSize = 40, BarAreaAlpha = 0};
            TotalChallengesChart = new DonutChart {Entries = challengesEntries, BackgroundColor = SKColor.Parse("#F2F2F7")};
        }
    }
}