using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class AddFriendViewModel: ViewModelBase
    {
        public RangeObservableCollection<UserViewModel> Users { get; private set; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand SearchCommand { get; }
        public IAsyncCommand LoadMoreCommand { get; }
        public ICommand SendFriendshipRequestCommand { get; set; }
        public ICommand RemoveFriendshipRequestCommand { get; set; }
        public bool IsRefreshing { get; set; }
        public int Skip { get; set; }

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
                        await Refresh();
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

        public AddFriendViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Users = new RangeObservableCollection<UserViewModel>();
            LoadMoreCommand = new AsyncCommand(LoadNotFriends);
            SearchCommand = new AsyncCommand(LoadNotFriends);
            RefreshCommand = new AsyncCommand(Refresh);
            SendFriendshipRequestCommand = new Command<UserViewModel>(SendFriendshipRequest);
            RemoveFriendshipRequestCommand = new Command<UserViewModel>(RemoveFriendshipRequest);

        }
        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                RefreshCommand.Execute(null);
        }

        private async Task Refresh()
        {
            Users.Clear();
            Skip = 0;
            await LoadNotFriends();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        private async Task LoadNotFriends()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var friends = await api.Users.GetNotFriends(SearchText, Skip);
                if (friends.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Users.AddRange(friends);
                api.ImageService.GetProfileImages(friends);
                
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

        public async void SendFriendshipRequest(UserViewModel obj)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie eine Anfrage stellen, um "  + obj.Name + " als Freund hinzuzufügen?",
                "Freund hinzufügen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.AskForFriendship(obj);
            if (!result)
            {
                dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }
        }

        public async void RemoveFriendshipRequest(UserViewModel obj)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie die Anfrage and " + obj.Name + " löschen?",
                "Freundschaftsanfrage löschen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.DeleteFriendshipRequest(obj);
            if (!result)
            {
                dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }
        }
    }
}