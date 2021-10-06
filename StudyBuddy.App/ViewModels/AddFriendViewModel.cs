using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class AddFriendViewModel: ViewModelBase
    {
        public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }
        public string SearchText { get; set; }

        public AddFriendViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            RefreshCommand = new Command(Reload);
            this.Reload();
        }

        public async void Reload()
        {
            Users = await api.Users.GetNotFriends(SearchText);
            NotifyPropertyChanged("Users");

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        public async void ApplyFilter()
        {
            Users = await api.Users.GetNotFriends(SearchText, false);
            NotifyPropertyChanged("Users");
        }
    }
}
