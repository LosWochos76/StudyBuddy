using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using StudyBuddy.Model.Model;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {

        private bool newsIsSelected = true;
        public bool NewsIsSelected { get => newsIsSelected; set { newsIsSelected = value; } }
        public bool RequestsIsSelected { get => newsIsSelected; set { newsIsSelected = !value; } }

        public ObservableCollection<RequestViewModel> Requests { get; private set; } = new ObservableCollection<RequestViewModel>();
        public NotificationsViewModelNews NewsViewModel { get; private set; }
        public ICommand RefreshCommand { get; }
        public bool IsRefreshing { get; set; }

        public ICommand RefreshNewsCommand { get; }
        public bool NewsIsRefreshing { get; set; }

        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DenyRequestCommand { get; set; }

        public NotificationsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.RefreshCommand = new Command(Refresh);
            this.AcceptRequestCommand = new Command<RequestViewModel>(AcceptRequest);
            this.DenyRequestCommand = new Command<RequestViewModel>(DenyRequest);
            
            this.NewsViewModel =  new NotificationsViewModelNews(api, dialog, navigation);
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
            {
                NewsViewModel.LoadNews();
                ReloadRequests();
            }
        }

        private async void Refresh()
        {
            if (newsIsSelected)
                await NewsViewModel.LoadNews();
            else
                await ReloadRequests();

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

   

        private async Task ReloadRequests()
        {
            try
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    api.Requests.ForMe(Requests, true);
                });
            }
            catch (ApiException e)
            {
                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
        }

        public async void AcceptRequest(RequestViewModel rvm)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie die " + rvm.TypeString + " annehmen?",
                "Anfrage annehmen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.Accept(rvm);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            await ReloadRequests();

            if (rvm.Type == Model.RequestType.Friendship)
            {
                var vm = TinyIoCContainer.Current.Resolve<FriendsViewModel>();
                vm.RefreshCommand.Execute(null);
            }
        }

        public async void DenyRequest(RequestViewModel rvm)
        {
            var answer = false;
            await dialog.ShowMessage(
                "Wollen Sie die " + rvm.TypeString + " ablehnen?",
                "Anfrage ablehnen?",
                "Ja", "Nein", a => { answer = a; });

            if (!answer)
                return;

            var result = await api.Requests.Deny(rvm);
            if (!result)
            {
                await dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            await ReloadRequests();

            if (rvm.Type == Model.RequestType.Friendship)
            {
                TinyIoCContainer.Current.Resolve<FriendsViewModel>().RefreshCommand.Execute(null);
            }
        }
    }

    public class NotificationsViewModelNews : ViewModelBase
    {
        public ObservableCollection<Notification> News { get; private set; } = new ObservableCollection<Notification>();


        public NotificationsViewModelNews(IApi api, IDialogService dialog, INavigationService navigation) : base(api,
            dialog, navigation)
        {
      
        }

        public async Task LoadNews()
        {
            var response = await this.api.Notifications.GetMyNotificationFeed();
            this.News.Clear();
            foreach (var notification in response)
            {
                this.News.Add(notification);
            }
        }
    }
    
}