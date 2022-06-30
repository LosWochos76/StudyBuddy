using System.Linq;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        public UserStatistics UserStatistics { get { return _userStatistic; } set { _userStatistic = value; NotifyPropertyChanged(); } }
        private UserStatistics _userStatistic;
        public BarChart TotalPointsChart { get { return _totalPointsChart; } set { _totalPointsChart = value; NotifyPropertyChanged(); } }
        private BarChart _totalPointsChart;
        public DonutChart TotalChallengesChart { get { return _totalChallengesChart; } set { _totalChallengesChart = value; NotifyPropertyChanged(); } }
        private DonutChart _totalChallengesChart;
        public int TotalBadges { get { return _totalBadges; } set { _totalBadges = value; NotifyPropertyChanged(); } }
        private int _totalBadges;
        public bool IsRefreshing { get; set; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand TotalChallengesCommand { get; set; }
        public IAsyncCommand TotalBadgeCommand { get; set; }
        public IAsyncCommand BadgeDetailsCommand { get; set; }
        public Color ThemeColor 
        { 
            get 
            { 
                switch (Application.Current.UserAppTheme)
                {
                    case OSAppTheme.Light: return Color.FromHex("#F2F2F7");
                    case OSAppTheme.Dark: return Color.Transparent;
                    default: return Color.Transparent;
                }
            }
        }

        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            TotalChallengesCommand = new AsyncCommand(ShowCompletedChallenges);
            TotalBadgeCommand = new AsyncCommand(ShowTotalBadge);
            RefreshCommand = new AsyncCommand(RefreshView);
            BadgeDetailsCommand = new AsyncCommand(ShowBadgeDetails);
        }

        private async Task ShowCompletedChallenges()
        {
            await Navigation.Push(new ChallengesCompletedPage());
        }

        private async Task ShowTotalBadge()
        {
            await Navigation.Push(new TotalBadgePage());
        }

        private async Task ShowBadgeDetails()
        {
            await Navigation.Push(new BadgeDetailsPage(await api.Badges.GetById(1)));
        }

        private async Task BadgesCount()
        {
            var badges = await api.Badges.BadgesReceived("", 0);
            _totalBadges = badges.Objects.Count();
        }

        private async Task RefreshView()
        {
            await Refresh();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        public async Task Refresh()
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
            await BadgesCount();
        }

        private void LoadCharts()
        {
            var pointsEntries = new[]
            {
                new ChartEntry(UserStatistics.TotalNetworkChallengesPoints)
                {
                    ValueLabel = UserStatistics.TotalNetworkChallengesPoints.ToString(),
                    Color = SKColor.Parse("#5856D6"),
                    ValueLabelColor = SKColor.Parse("#5856D6")
                },
                new ChartEntry(UserStatistics.TotalOrganizingChallengesPoints)
                {
                    ValueLabel = UserStatistics.TotalOrganizingChallengesPoints.ToString(),
                    Color = SKColor.Parse("#5AC8FA"),
                    ValueLabelColor = SKColor.Parse("#5AC8FA")
                },
                new ChartEntry(UserStatistics.TotalLearningChallengesPoints)
                {
                    ValueLabel = UserStatistics.TotalLearningChallengesPoints.ToString(),
                    Color = SKColor.Parse("#007AFF"),
                    ValueLabelColor = SKColor.Parse("#007AFF")
                }
            };
            var challengesEntries = new[]
            {
                new ChartEntry(UserStatistics.TotalNetworkChallengesCount) { Color = SKColor.Parse("#5856D6") },
                new ChartEntry(UserStatistics.TotalOrganizingChallengesCount) { Color = SKColor.Parse("#5AC8FA") },
                new ChartEntry(UserStatistics.TotalLearningChallengesCount) { Color = SKColor.Parse("#007AFF") }
            };
            TotalPointsChart = new BarChart
            {
                Entries = pointsEntries,
                LabelTextSize = 0,
                ValueLabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                BarAreaAlpha = 0,
                BackgroundColor = new SKColor(0,0,0,0)
            };
            TotalChallengesChart = new DonutChart {Entries = challengesEntries, BackgroundColor = new SKColor(0,0,0,0)};
        }
    }
}