using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.CommunityToolkit.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class GameBadgesViewModel : ViewModelBase
    {
        public RangeObservableCollection<GameBadgeViewModel> Badges { get; private set; }
        public GameBadgeViewModel SelectedBadge { get; set; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand DetailsCommand { get; }
        public IAsyncCommand SearchCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public int Skip { get; set; }
        public bool IsBusy { get; set; } = false;

        public GameBadgesViewModel(IApi api) : base(api)
        {
            Badges = new RangeObservableCollection<GameBadgeViewModel>();
            SearchCommand = new AsyncCommand(Refresh);
            RefreshCommand = new AsyncCommand(Refresh);
            DetailsCommand = new AsyncCommand(ShowDetails);
        }

        private string search_text = string.Empty;
        public string SearchText
        {
            get { return search_text; }
            set
            {
                if (search_text == value)
                    return;

                search_text = value ?? string.Empty;
                Task.Run(async () =>
                {
                    string SearchText = search_text;
                    await Task.Delay(1000);

                    if (search_text == SearchText)
                        await Refresh();
                });
            }
        }

        private int item_treshold = 1;
        public int ItemThreshold
        {
            get { return item_treshold; }
            set
            {
                item_treshold = value;
                NotifyPropertyChanged();
            }
        }

        private async Task ShowDetails()
        {
            
            if (SelectedBadge == null)
                return;

            await api.Device.PushPage(new BadgeDetailsPage(await api.Badges.GetById(SelectedBadge.ID)));
            SelectedBadge = null;
            NotifyPropertyChanged(nameof(SelectedBadge));
            
        }
        private async Task Refresh()
        {
            Badges.Clear();
            Skip = 0;
            await LoadBadges();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        private async Task LoadBadges()
        {
            if (IsBusy)
                return;
            else
                IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var badges = await api.Badges.BadgesReceived(api.Authentication.CurrentUser.ID, SearchText, Skip);
                if (badges.Objects.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Badges.AddRange(badges.Objects);
                Skip += 10;
            }
            catch (ApiException e)
            {
                api.Device.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}