using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase
    {
        public RangeObservableCollection<NotificationViewModel> Notifications { get; } = new RangeObservableCollection<NotificationViewModel>();
        public bool IsRefreshing { get; set; }
        public int Skip { get; set; }
        public bool IsBusy { get; set; } = false;
        public ICommand RefreshCommand { get; set; }
        public ICommand LoadMoreCommand { get; set; }
        public ICommand RefreshNewsCommand { get; }
        public ICommand OpenCommentsCommands { get; set; }

        public NotificationsPageViewModel(IApi api) : base(api)
        {
            RefreshCommand = new AsyncCommand(Refresh);
            LoadMoreCommand = new AsyncCommand(LoadNews);
            OpenCommentsCommands = new Command<NotificationViewModel>(OpenComments);
        }

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

        private async Task Refresh()
        {
            Notifications.Clear();
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
                var objects = await api.Notifications.GetNotificationsForFriends(Skip);
                if (objects.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Notifications.AddRange(objects);
                Skip += 10;
            }
            catch (ApiException e)
            {
                api.Device.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OpenComments(NotificationViewModel notification)
        {
            api.Device.PushPage(new CommentModalPage(notification));
        }
    }
}