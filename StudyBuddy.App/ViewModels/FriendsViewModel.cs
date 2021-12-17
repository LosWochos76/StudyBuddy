using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class FriendsViewModel : ViewModelBase
    {
        private int skip = 0;
        public RangeObservableCollection<UserViewModel> Friends { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand AddFriendCommand { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public bool IsRefreshing { get; set; } = false;

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

        public FriendsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Friends = new RangeObservableCollection<UserViewModel>();
            LoadMoreCommand = new Command(LoadFriends);
            SearchCommand = new Command(Refresh);
            RefreshCommand = new Command(Refresh);
            DetailsCommand = new Command<UserViewModel>(Details);
            AddFriendCommand = new Command(AddFriend);

            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            api.FriendshipStateChanged += Api_FriendshipStateChanged;
            api.RequestStateChanged += Api_RequestStateChanged;
        }

        private void Api_RequestStateChanged(object sender, RequestStateChangedEventArgs e)
        {
            if (e.Request.Type == Model.RequestType.Friendship)
                Refresh();
        }

        private void Api_FriendshipStateChanged(object sender, FriendshipStateChangedEventArgs e)
        {
            Refresh();
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Refresh();
        }

        private void Refresh()
        {
            Friends.Clear();
            skip = 0;
            LoadFriends();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        private async void LoadFriends()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var friends = await api.Users.GetFriends(SearchText, skip);
                if (friends.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Friends.AddRange(friends);
                api.ImageService.GetProfileImages(friends);
                skip += 10;
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

        public async void Details(UserViewModel obj)
        {
            var userStatistics = await api.Statistics.GetUserStatisticsForUser(obj.ID);
            navigation.Push(new FriendPage(obj,userStatistics));
        }

        public void AddFriend()
        {
            navigation.Push(new AddFriendPage());
        }
    }
}