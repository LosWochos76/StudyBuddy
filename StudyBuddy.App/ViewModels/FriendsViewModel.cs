using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class FriendsViewModel : ViewModelBase
    {
        public RangeObservableCollection<UserViewModel> Friends { get; private set; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand DetailsCommand { get; }
        public IAsyncCommand AddFriendCommand { get; set; }
        public IAsyncCommand SearchCommand { get; }
        public IAsyncCommand LoadMoreCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public int Skip { get; set; }
        public UserViewModel SelectedUser { get; set; }

        private string search_text = string.Empty;
        public string SearchText
        {
            get { return search_text; }
            set
            {
                if (search_text == value)
                    return;

                search_text = value ?? string.Empty;
                Task.Run(async () =>
                {
                    string SearchText = search_text;
                    await Task.Delay(1000);

                    if (search_text == SearchText)
                        Refresh();
                });
            }
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
        private bool is_busy;
        public bool IsBusy
        {
            get { return is_busy; }
            set
            {
                is_busy = value;
                NotifyPropertyChanged(nameof(IsBusy));
            }
        }


        public FriendsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Friends = new RangeObservableCollection<UserViewModel>();
            LoadMoreCommand = new AsyncCommand(LoadFriends);
            SearchCommand = new AsyncCommand(Refresh);
            RefreshCommand = new AsyncCommand(Refresh);
            DetailsCommand = new AsyncCommand(Details);
            AddFriendCommand = new AsyncCommand(AddFriend);

            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            api.FriendshipStateChanged += Api_FriendshipStateChanged;
            api.RequestStateChanged += Api_RequestStateChanged;
        }

        private void Api_RequestStateChanged(object sender, RequestStateChangedEventArgs e)
        {
            if (e.Request.Type == Model.RequestType.Friendship)
                RefreshCommand.Execute(null);
        }

        private void Api_FriendshipStateChanged(object sender, FriendshipStateChangedEventArgs e)
        {
            RefreshCommand.Execute(null);
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                RefreshCommand.Execute(null);
        }

        private async Task Refresh()
        {
            Friends.Clear();
            Skip = 0;
            await LoadFriends();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        private async Task LoadFriends()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var friends = await api.Users.GetFriends(SearchText, Skip);
                if (friends.Objects.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Friends.AddRange(friends.Objects);
                await api.ImageService.GetProfileImages(friends.Objects);
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

        private async Task Details()
        {
            if (SelectedUser == null)
                return;

            var userStatistics = await api.Statistics.GetUserStatisticsForUser(SelectedUser.ID);
            await Navigation.Push(new FriendPage(SelectedUser, userStatistics));
            SelectedUser = null;
            NotifyPropertyChanged(nameof(SelectedUser));
        }

        public async Task AddFriend()
        {
            var route = $"{nameof(AddFriendPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}