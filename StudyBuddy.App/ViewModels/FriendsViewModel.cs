using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class FriendsViewModel : ViewModelBase
    {
        public ObservableCollection<UserViewModel> Friends { get; private set; } = new ObservableCollection<UserViewModel>();
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand AddFriendCommand { get; set; }
        public string SearchText { get; set; }

        public FriendsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.RefreshCommand = new Command(Reload);
            this.DetailsCommand = new Command<UserViewModel>(Details);
            this.AddFriendCommand = new Command(AddFriend);
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            api.FriendshipStateChanged += Api_FriendshipStateChanged;
            api.RequestStateChanged += Api_RequestStateChanged;
        }

        private void Api_RequestStateChanged(object sender, RequestStateChangedEventArgs e)
        {
            if (e.Request.Type == Model.RequestType.Friendship)
                Reload();
        }

        private void Api_FriendshipStateChanged(object sender, FriendshipStateChangedEventArgs e)
        {
            Reload();
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            try
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    api.Users.GetFriends(Friends, SearchText, true);
                });
            }
            catch (Exception e)
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
                api.Users.GetFriends(Friends, SearchText, false);
            });
        }

        public async void Details(UserViewModel obj)
        {
            await navigation.Push(new FriendPage(obj));
        }

        public async void AddFriend()
        {
            await navigation.Push(new AddFriendPage());
        }
    }
}