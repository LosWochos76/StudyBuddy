using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using Xamarin.CommunityToolkit.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class FriendViewModel : ViewModelBase
    {
        public UserViewModel User { get; set; }
        public IAsyncCommand RemoveFriendCommand { get; set; }
        public IAsyncCommand ShowFriendsStatisticCommand { get; set; }
        public UserStatistics UserStatistics { get; set; }
        public BarChart TotalPointsChart { get { return _totalPointsChart; } set { _totalPointsChart = value; NotifyPropertyChanged(); } }
        private BarChart _totalPointsChart;

        public FriendViewModel(IApi api, IDialogService dialog, INavigationService navigation, UserViewModel obj, UserStatistics userStatistics) : base(api, dialog, navigation)
        {
            User = obj;
            UserStatistics = userStatistics;
            RemoveFriendCommand = new AsyncCommand(RemoveFriend);
            ShowFriendsStatisticCommand = new AsyncCommand(ShowCompletedChallenges);
            LoadCharts();
        }

        private async Task ShowCompletedChallenges()
        {
            await Navigation.Push(new StatisticPage());
        }

        public async Task RemoveFriend()
        {
            var answer = await dialog.ShowMessage(
                "Wollen Sie " + User.Firstname + " als Freund entfernen?",
                "Freund entfernen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            var result = await api.Users.RemoveFriend(User);

            if (!result)
            {
                dialog.ShowError("Fehler!", "Der Freund konnte nicht entfernt werden!", "Ok", null);
                return;
            }

            await Navigation.Pop();
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
            TotalPointsChart = new BarChart
            {
                Entries = pointsEntries,
                LabelTextSize = 0,
                ValueLabelTextSize = 48,
                ValueLabelOrientation = Orientation.Horizontal,
                BarAreaAlpha = 0,
                BackgroundColor = new SKColor(0,0,0,0)
            };
        }
    }
}