using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengesViewModel : ViewModelBase
    {
        public RangeObservableCollection<ChallengeViewModel> Challenges { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand ScanQrCodeCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand SearchCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public string Header => string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());
        private string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value ?? string.Empty;
                    NotifyPropertyChanged(nameof(SearchText));
                    if (SearchCommand.CanExecute(null))
                    {
                        SearchCommand.Execute(null);
                    }
                }
            }
        }
        public int Skip { get; set; }
        public bool IsBusy { get; private set; } = false;
        public int ItemThreshold { get; set; } = 1;
        public int PageNo { get; set; } = 0;

        public ChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Challenges = new RangeObservableCollection<ChallengeViewModel>();
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            LoadMoreCommand = new Command(async () => await ItemsThresholdReached());
            SearchCommand = new Command(async () => await LoadChallengesCommand());
            RefreshCommand = new Command(async () =>
            {
                await LoadChallengesCommand();
                IsRefreshing = false;
                NotifyPropertyChanged(nameof(IsRefreshing));
            });
            DetailsCommand = new Command<ChallengeViewModel>(ShowDetails);
            ScanQrCodeCommand = new Command(ScanQrCode);
            api.ChallengeAccepted += (sender, e) => { _ = LoadChallengesCommand(); };
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
              RefreshCommand.Execute(null);
        }

        async Task LoadChallengesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                Challenges.Clear();
                var challenges = await api.Challenges.ForToday(SearchText);
                Challenges.AddRange(challenges);
                PageNo = 1;
                Skip = 10;
            }
            catch (ApiException e)
            {
                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task ItemsThresholdReached()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Skip = 10 * PageNo;
                var items = await api.Challenges.ForToday(SearchText, Skip);
                Challenges.AddRange(items);

                if (items.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                PageNo++;
            }
            catch (ApiException e)
            {
                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ShowDetails(ChallengeViewModel obj)
        {
            navigation.Push(new ChallengeDetailsPage(obj));
        }

        private async void ScanQrCode()
        {
            var route = $"{nameof(QrCodePage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}