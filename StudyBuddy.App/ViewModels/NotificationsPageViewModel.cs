using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase
    {
        public IApi Api;
        public CollectionView NotificationCollectionView;

        public NotificationsPageViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api,
            dialog, navigation)
        {
            RefreshCommand = new Command(Refresh);
            AcceptRequestCommand = new Command<RequestViewModel>(AcceptRequest);
            DenyRequestCommand = new Command<RequestViewModel>(DenyRequest);
            NewsDetailCommand = new Command(() => { });
            NewsRemainingItemsThresholdReachedCommand = new Command(LoadNews);
            Api = api;
        }

        public Command<NewsViewModel> OpenCommentsCommands { get; set; }

        public bool NewsIsSelected { get; set; } = true;

        public bool RequestsIsSelected
        {
            get => NewsIsSelected;
            set => NewsIsSelected = !value;
        }

        public RangeObservableCollection<NewsViewModel> News { get; } = new();
        public RangeObservableCollection<RequestViewModel> Requests { get; } = new();

        public ICommand RefreshCommand { get; set; }
        public bool IsRefreshing { get; set; }


        public int NewsCount { get; } = 10;
        public int NewsStart { get; set; }
        public ICommand RefreshNewsCommand { get; }
        public bool NewsIsRefreshing { get; set; }
        public int NewsRemainingItemsThreshold { get; set; } = 20;
        public bool IsLoadingNews { get; set; }

        public ICommand NewsDetailCommand { get; set; }
        public NewsViewModel NewsSelectedItem { get; set; }
        public ICommand NewsRemainingItemsThresholdReachedCommand { get; set; }


        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DenyRequestCommand { get; set; }


        private void Refresh()
        {
            LoadNews();
            ReloadRequests();

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        private async void ReloadRequests()
        {
            var response = await api.Requests.ForMe();
            if (response is null)
                return;

            Requests.Clear();
            Requests.AddRange(response.Objects);
            api.ImageService.GetProfileImages(Requests);
        }

        public async void AcceptRequest(RequestViewModel rvm)
        {
            var answer = await dialog.ShowMessage(
                "Wollen Sie die " + rvm.TypeString + " annehmen?",
                "Anfrage annehmen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            var result = await api.Requests.Accept(rvm);
            if (!result)
            {
                dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            ReloadRequests();
        }

        public async void DenyRequest(RequestViewModel rvm)
        {
            var answer = await dialog.ShowMessage(
                "Wollen Sie die " + rvm.TypeString + " ablehnen?",
                "Anfrage ablehnen?",
                "Ja", "Nein", null);

            if (!answer)
                return;

            var result = await api.Requests.Deny(rvm);
            if (!result)
            {
                dialog.ShowError("Ein Fehler ist aufgetreten!", "Fehler!", "Ok", null);
                return;
            }

            ReloadRequests();

            if (rvm.Type == RequestType.Friendship)
                TinyIoCContainer.Current.Resolve<FriendsViewModel>().RefreshCommand.Execute(null);
        }

        public async void LoadNews()
        {
            if (IsLoadingNews)
            {
                return;
            }

            IsLoadingNews = true;

            var filter = new NotificationFilter()
            {
                Start = NewsStart,
                Count = NewsCount
            };
            
            var response = await this.api.Notifications.GetMyNotificationFeed(filter);

            if (response.Count() == 0)
            {
                NewsRemainingItemsThreshold = -1;
                return;
            }

            if (response.Count() < filter.Count)
            {
                NewsRemainingItemsThreshold = -1;
            }

            if (response is null)
            {
                IsLoadingNews = false;
                return;
            }
           
            News.AddRange(response);
            NewsStart += 10;
            IsLoadingNews = false;


        }
    }
}