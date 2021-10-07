using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class AddFriendViewModel: ViewModelBase
    {
        public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; }
        public string SearchText { get; set; }
        public ICommand AddFriendCommand { get; set; }

        public AddFriendViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            RefreshCommand = new Command(Reload);
            AddFriendCommand = new Command<UserViewModel>(AddFriend);
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

        public async void AddFriend(UserViewModel obj)
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
    }
}