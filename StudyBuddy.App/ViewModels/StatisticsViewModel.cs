using System.Linq;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;
using StudyBuddy.App.Api;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
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
        
        private GameBadgeViewModel _lastBadge;
        public GameBadgeViewModel LastBadge{ get { return _lastBadge; } set { _lastBadge = value; NotifyPropertyChanged(); } }
        
        private bool _hasValue = false;
        public bool HasValue{ get { return _hasValue; } set { _hasValue = value; NotifyPropertyChanged(); } }
        
        private int _totalBadges;
        public int TotalBadges { get { return _totalBadges; } set { _totalBadges = value; NotifyPropertyChanged(); } }
        
        public bool IsRefreshing { get; set; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand TotalChallengesCommand { get; set; }
        public IAsyncCommand TotalBadgeCommand { get; set; }
        public IAsyncCommand BadgeDetailsCommand { get; set; }

        public StatisticsViewModel(IApi api) : base(api)
        {
            TotalChallengesCommand = new AsyncCommand(ShowCompletedChallenges);
            TotalBadgeCommand = new AsyncCommand(ShowTotalBadge);
            RefreshCommand = new AsyncCommand(RefreshView);

            BadgeDetailsCommand = new AsyncCommand(execute: async () =>
            {
                await ShowBadgeDetails();
            },
            canExecute: () =>
            {
                if (TotalBadges < 1)
                    return false;
                else
                    return true;
            });
        }

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

        private async Task ShowCompletedChallenges()
        {
            await api.Device.PushPage(new ChallengesCompletedPage());
        }

        private async Task ShowTotalBadge()
        {
            await api.Device.PushPage(new TotalBadgePage());
        }

        private async Task ShowBadgeDetails()
        {
            await api.Device.PushPage(new BadgeDetailsPage(LastBadge));
        }
        
        private async Task RefreshView()
        {
            await Refresh();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        public async Task Refresh()
        {
            GameBadgeViewModel emptybadge = new GameBadgeViewModel{ Name = "Verdiene Abzeichen", IconKey = "fa-solid,f70c,#3700B3" };
            try
            {
                var badges = await api.Badges.BadgesReceived();
                if (badges.Count < 1)
                {
                    LastBadge = emptybadge;
                    TotalBadges = 0;
                }
                else
                {
                    var firstElement = badges.Objects.FirstOrDefault();
                    LastBadge = firstElement;
                    TotalBadges = badges.Count;
                    BadgeDetailsCommand.CanExecute(true);
                }
                UserStatistics = await api.Statistics.GetUserStatistics();
            }
            catch (System.Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Fehler",
                    "Fehler beim Laden der Statistiken. API Endpunkt nicht erreichbar", "Ok");
            }

            if (UserStatistics.ThisWeekChallengeCount != 0 || UserStatistics.LastWeekChallengeCount != 0)
            {
                HasValue = true;
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