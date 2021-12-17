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
        private int skip = 0;

        public RangeObservableCollection<ChallengeViewModel> Challenges { get; private set; }
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand ScanQrCodeCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand SearchCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public string Header => string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());
        public ChallengeViewModel SelectedChallenge { get; set; }

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
                        Refresh();
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

        public ChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Challenges = new RangeObservableCollection<ChallengeViewModel>();
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            api.ChallengeAccepted += (sender, e) => { LoadChallenges(); };

            LoadMoreCommand = new Command(LoadChallenges);
            SearchCommand = new Command(Refresh);
            DetailsCommand = new Command(ShowDetails);
            ScanQrCodeCommand = new Command(ScanQrCode);
            RefreshCommand = new Command(Refresh);
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
              RefreshCommand.Execute(null);
        }

        private void Refresh()
        {
            Challenges.Clear();
            skip = 0;
            LoadChallenges();
            IsRefreshing = false;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }

        private async void LoadChallenges()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ItemThreshold = 1;
                var challenges = await api.Challenges.ForToday(SearchText, skip);
                if (challenges.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }

                Challenges.AddRange(challenges);
                skip += 10;
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

        private async void ShowDetails()
        {
            if (SelectedChallenge == null)
                return;
            
            navigation.Push(new ChallengeDetailsPage(SelectedChallenge));
            SelectedChallenge = null;
        }

        private async void ScanQrCode()
        {
            var route = $"{nameof(QrCodePage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}