using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class AddFriendViewModel: ViewModelBase
    {
        public RangeObservableCollection<UserViewModel> Users { get; private set; }
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand SendFriendshipRequestCommand { get; set; }
        public ICommand RemoveFriendshipRequestCommand { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand LoadMoreCommand { get; }
        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value)
                    return;

                _searchText = value ?? string.Empty;
                Task.Run(async () =>
                {
                    string SearchText = _searchText;
                    await Task.Delay(1000);
                    if (_searchText == SearchText)
                        await LoadNotFriendsCommand();
                });
            }
        }
        public int Skip { get; set; }
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged(nameof(IsBusy));
            }
        }
        public int ItemThreshold { get; set; } = 1;
        public int PageNo { get; set; } = 0;

        public AddFriendViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Users = new RangeObservableCollection<UserViewModel>();
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            LoadMoreCommand = new Command(async () => await ItemsThresholdReached());
            SearchCommand = new Command(async () => await LoadNotFriendsCommand());
            RefreshCommand = new Command(async () =>
            {
                await LoadNotFriendsCommand();
                IsRefreshing = false;
                NotifyPropertyChanged(nameof(IsRefreshing));
            });
            SendFriendshipRequestCommand = new Command<UserViewModel>(SendFriendshipRequest);
            RemoveFriendshipRequestCommand = new Command<UserViewModel>(RemoveFriendshipRequest);
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                RefreshCommand.Execute(null);
        }

        async Task LoadNotFriendsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                Users.Clear();
                var friends = await api.Users.GetNotFriends(SearchText);
                Users.AddRange(friends);
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
                var friends = await api.Users.GetNotFriends(SearchText, Skip);
                Users.AddRange(friends);

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
        public async void SendFriendshipRequest(UserViewModel obj)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie eine Anfrage stellen, um "  + obj.Nickname + " als Freund hinzuzufügen?",
                "Freund hinzufügen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.AskForFriendship(obj);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }
        }

        public async void RemoveFriendshipRequest(UserViewModel obj)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie die Anfrage and " + obj.Nickname + " löschen?",
                "Freundschaftsanfrage löschen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.DeleteFriendshipRequest(obj);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }
        }
    }
}