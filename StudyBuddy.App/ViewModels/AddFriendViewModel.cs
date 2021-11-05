using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class AddFriendViewModel: ViewModelBase
    {
        public ObservableCollection<UserViewModel> Users { get; private set; } = new ObservableCollection<UserViewModel>();
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }
        public string SearchText { get; set; }
        public ICommand SendFriendshipRequestCommand { get; set; }
        public ICommand RemoveFriendshipRequestCommand { get; set; }

        public AddFriendViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            RefreshCommand = new Command(Reload);
            SendFriendshipRequestCommand = new Command<UserViewModel>(SendFriendshipRequest);
            RemoveFriendshipRequestCommand = new Command<UserViewModel>(RemoveFriendshipRequest);

            api.FriendshipStateChanged += Api_FriendshipStateChanged;
            api.RequestStateChanged += Api_RequestStateChanged;

            Reload();
        }

        private void Api_RequestStateChanged(object sender, RequestStateChangedEventArgs e)
        {
            Reload();
        }

        private void Api_FriendshipStateChanged(object sender, FriendshipStateChangedEventArgs e)
        {
            Reload();
        }

        public async void Reload()
        {
            try
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    api.Users.GetNotFriends(Users, SearchText, true);
                });
            }
            catch (ApiException e)
            {
                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        public async void ApplyFilter()
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                api.Users.GetNotFriends(Users, SearchText, false);
                NotifyPropertyChanged("Users");
            });
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

            var result = await api.Requests.AskForFriendship(obj.ID);
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

            var result = await api.Requests.Delete(obj.FriendshipRequest);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }
        }
    }
}