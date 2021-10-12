using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        public ObservableCollection<NotificationBaseViewModel> Notifications { get; private set; } = new ObservableCollection<NotificationBaseViewModel>();
        public ICommand RefreshCommand { get; }
        public bool IsRefreshing { get; set; }
        public ICommand AcceptCommand { get; set; }
        public ICommand DenyCommand { get; set; }

        public NotificationsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.RefreshCommand = new Command(Reload);
            this.AcceptCommand = new Command<NotificationBaseViewModel>(Accept);
            this.DenyCommand = new Command<NotificationBaseViewModel>(Deny);
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        private async void Reload()
        {
            var requests = await api.Requests.ForMe();
            Notifications.Clear();
            foreach (var obj in requests)
                Notifications.Add(obj);

            // Now load all News and add them to the List

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        public void Accept(NotificationBaseViewModel model)
        {
            if (model is RequestViewModel)
            {
                var rvm = (RequestViewModel)model;
                AcceptRequest(rvm);
            }
        }

        private async void AcceptRequest(RequestViewModel rvm)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie die " + rvm.TypeString + " annehmen?",
                "Anfrage annehmen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.Accept(rvm.ID);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            Reload();

            if (rvm.Type == Model.RequestType.Friendship)
            {
                var vm = TinyIoCContainer.Current.Resolve<FriendsViewModel>();
                vm.Reload();
            }
        }

        public void Deny(NotificationBaseViewModel model)
        {
            if (model is RequestViewModel)
            {
                var rvm = (RequestViewModel)model;
                DenyRequest(rvm);
            }
        }

        private async void DenyRequest(RequestViewModel rvm)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie die " + rvm.TypeString + " ablehnen?",
                "Anfrage ablehnen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.Deny(rvm.ID);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            Reload();

            if (rvm.Type == Model.RequestType.Friendship)
            {
                var vm = TinyIoCContainer.Current.Resolve<FriendsViewModel>();
                vm.Reload();
            }
        }
    }
}