using System;
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
        public RangeObservableCollection<UserViewModel> Friends { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand AddFriendCommand { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value ?? string.Empty;
                    NotifyPropertyChanged(nameof(SearchText));
                    if (SearchCommand.CanExecute(null))
                    {
                        SearchCommand.Execute(null);
                    }
                }
            }
        }
        public int Skip { get; set; }
        public bool IsBusy { get; private set; } = false;
        public int ItemThreshold { get; set; } = 1;
        public int PageNo { get; set; } = 0;

        public FriendsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Friends = new RangeObservableCollection<UserViewModel>();
            LoadMoreCommand = new Command(async () => await ItemsThresholdReached());
            SearchCommand = new Command(async () => await LoadFriendsCommand());
            RefreshCommand = new Command(async () =>
            {
                await LoadFriendsCommand();
                IsRefreshing = false;
                NotifyPropertyChanged(nameof(IsRefreshing));
            });
            DetailsCommand = new Command<UserViewModel>(Details);
            AddFriendCommand = new Command(AddFriend);
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

        async Task LoadFriendsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                Friends.Clear();
                var friends = await api.Users.GetFriends(SearchText);
                Friends.AddRange(friends);
                PageNo = 1;
                Skip = 10;
            }
            catch (ApiException e)
            {
                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task ItemsThresholdReached()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Skip = 10 * PageNo;
                var friends = await api.Users.GetFriends(SearchText, Skip);
                Friends.AddRange(friends);

                if (friends.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                PageNo++;
            }
            catch (ApiException e)
            {
                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async void Details(UserViewModel obj)
        {
            var userStatistics = await api.Statistics.GetUserStatisticsForUser(obj.ID);
            await navigation.Push(new FriendPage(obj,userStatistics));
        }

        public async void AddFriend()
        {
            await navigation.Push(new AddFriendPage());
        }
    }
}