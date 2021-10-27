using System;
using System.Collections.Generic;
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
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            if (!IsRefreshing)
            {
                IsRefreshing = true;
                NotifyPropertyChanged("IsRefreshing");
            }

            try
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    api.Users.GetFriends(Friends, SearchText, true);
                    NotifyPropertyChanged("Friends");
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
                NotifyPropertyChanged("Friends");
            });
        }

        public void Details(UserViewModel obj)
        {
            navigation.Push(new FriendPage(obj));
        }

        public void AddFriend()
        {
            navigation.Push(new AddFriendPage());
        }
    }
}
