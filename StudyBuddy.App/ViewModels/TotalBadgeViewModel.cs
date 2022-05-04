using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class TotalBadgeViewModel : ViewModelBase
    {
        public string Header => $"Abgeschlossene Badges am {DateTime.Now.ToShortDateString()}";
        public RangeObservableCollection<GameBadge> Badges { get; private set; }
        public GameBadge SelectedBadge { get; set; }
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand LoadMoreCommand { get; }
        public IAsyncCommand SearchCommand { get; }
        public IAsyncCommand DetailsCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public int Skip { get; set; }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value)
                    return;

                _searchText = value ?? string.Empty;
                Task.Run(async () =>
                {
                    string searchText = _searchText;
                    await Task.Delay(1000);

                    if (_searchText == searchText)
                        await Refresh();
                });
            }
        }
        private string _searchText = string.Empty;
        public int ItemThreshold
        {
            get { return _itemTreshold; }
            set
            {
                _itemTreshold = value;
                NotifyPropertyChanged();
            }
        }
        private int _itemTreshold = 1;
        
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged();
            }
        }
        private bool _isBusy = false;

        public TotalBadgeViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Badges = new RangeObservableCollection<GameBadge>();
            LoadMoreCommand = new AsyncCommand(LoadBadges);
            SearchCommand = new AsyncCommand(Refresh);
            RefreshCommand = new AsyncCommand(Refresh);
            DetailsCommand = new AsyncCommand(ShowDetails);
        }
        private async Task ShowDetails()
        {
            /*
            if (SelectedBadge == null)
                return;

            await Navigation.Push(new BadgeCompletedDetailsPage(SelectedBadge));
            SelectedBadge = null;
            NotifyPropertyChanged(nameof(SelectedBadge));
            */
        }
        public async Task Refresh()
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
            IsBusy = true;
            
            try
            {
                ItemThreshold = 1;
                var badges = await api.Badges.Accepted(SearchText, Skip);
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
                dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}