using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using TinyIoC;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class RequestsViewModel : ViewModelBase
    {
        public RangeObservableCollection<RequestViewModel> Requests { get; private set; }
        public IAsyncCommand RefreshCommand { get; }
        public ICommand AcceptRequestCommand { get; set; }
        public ICommand DenyRequestCommand { get; set; }
        public bool IsRefreshing { get; set; } = false;

        private bool is_busy = false;
        public bool IsBusy
        {
            get { return is_busy; }
            set
            {
                is_busy = value;
                NotifyPropertyChanged();
            }
        }

        public RequestsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Requests = new RangeObservableCollection<RequestViewModel>();
            RefreshCommand = new AsyncCommand(Refresh);
            AcceptRequestCommand = new Command<RequestViewModel>(AcceptRequest);
            DenyRequestCommand = new Command<RequestViewModel>(DenyRequest);
        }

        private async Task Refresh()
        {
            Requests.Clear();
            await LoadRequests();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        private async Task LoadRequests()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                var requests = await api.Requests.ForMe();
                if (requests.Count == 0)
                    return;
                Requests.AddRange(requests.Objects);
                await api.ImageService.GetProfileImages(Requests);
            }
            catch(ApiException e)
            {
                dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
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

            await Refresh();
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

            await Refresh();

            if (rvm.Type == Model.RequestType.Friendship)
            {
                TinyIoCContainer.Current.Resolve<FriendsViewModel>().RefreshCommand.Execute(null);
            }
        }
    }

    
}
