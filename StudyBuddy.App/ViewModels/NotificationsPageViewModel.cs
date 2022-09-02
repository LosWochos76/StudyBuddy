using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase
    {
        public IApi Api;
        public Command<NewsViewModel> OpenCommentsCommands { get; set; }
        public RangeObservableCollection<NewsViewModel> News { get; } = new RangeObservableCollection<NewsViewModel>();
        public ICommand RefreshCommand { get; set; }
        public ICommand LoadMoreCommand { get; set; }
        public ICommand RefreshNewsCommand { get; }
        public bool IsRefreshing { get; set; }
        public int Skip { get; set; }
        public bool IsBusy { get; set; } = false;

        private int item_treshold = 1;
        public int ItemThreshold
        {
            get { return item_treshold; }
            set
            {
                item_treshold = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand NewsDetailCommand { get; set; }
        public NewsViewModel NewsSelectedItem { get; set; }
        public ICommand NewsRemainingItemsThresholdReachedCommand { get; set; }

        public NotificationsPageViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            RefreshCommand = new AsyncCommand(Refresh);
            NewsDetailCommand = new Command(() => { });
            LoadMoreCommand = new AsyncCommand(LoadNews);
            Api = api;
        }

        private async Task Refresh()
        {
            News.Clear();
            Skip = 0;
            await LoadNews();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        public async Task LoadNews()
        {
            if (IsBusy)
                return;
            else
                IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var news = await api.Notifications.GetMyNotificationFeed(Skip);
                if (news.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                News.AddRange(news);
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