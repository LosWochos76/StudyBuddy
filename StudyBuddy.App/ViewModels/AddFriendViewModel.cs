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
        private int skip = 0;
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
                        Refresh();
                });
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

        private int item_treshold = 0;
        public int ItemThreshold
        {
            get { return item_treshold; }
            set
            {
                item_treshold = value;
                NotifyPropertyChanged();
            }
        }

        public AddFriendViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Users = new RangeObservableCollection<UserViewModel>();
            LoadMoreCommand = new Command(LoadNotFriends);
            SearchCommand = new Command(LoadNotFriends);
            RefreshCommand = new Command(Refresh);
            SendFriendshipRequestCommand = new Command<UserViewModel>(SendFriendshipRequest);
            RemoveFriendshipRequestCommand = new Command<UserViewModel>(RemoveFriendshipRequest);

            Refresh();
        }

        private void Refresh()
        {
            Users.Clear();
            skip = 0;
            LoadNotFriends();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        private async void LoadNotFriends()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var friends = await api.Users.GetNotFriends(SearchText, skip);
                if (friends.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Users.AddRange(friends);
                api.ImageService.GetProfileImages(friends);
                api.Requests.AddFriendshipRequests(friends);
                skip += 10;
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