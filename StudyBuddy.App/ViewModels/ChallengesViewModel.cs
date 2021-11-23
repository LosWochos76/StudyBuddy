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
        public ObservableCollection<ChallengeViewModel> Challenges { get; private set; }
        public ICommand LoadAllChallengesCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand ScanQrCodeCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public bool IsRefreshing { get; set; } = false;
        public string Header => string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());
        public string SearchText { get; set; }
        private int Skip { get; set; }
        public bool IsBusy { get; private set; } = false;
        public int ItemThreshold { get; set; } = 1;
        

        public ChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            Challenges = new ObservableCollection<ChallengeViewModel>();
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            LoadAllChallengesCommand = new Command(async () => await LoadChallengesCommand());
            LoadMoreCommand = new Command(async () => await ItemsThresholdReached());
            RefreshCommand = new Command(async () =>
            {
                await LoadChallengesCommand();
                IsRefreshing = false;
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
                Skip = 10;
                Challenges.Clear();
                var challenges = await api.Challenges.ForToday(SearchText);
                Challenges.Add(challenges);
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

        public async void ApplyFilter()
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                api.Challenges.ForToday(SearchText);
            });
        }

        async Task ItemsThresholdReached()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await api.Challenges.LoadMore(SearchText, Skip);
                Challenges.Add(items);

                if (items.Count() == 0)
                {
                    ItemThreshold = -1;
                    return;
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                Skip += 10;
                IsBusy = false;
            }
        }

        private void ShowDetails(ChallengeViewModel obj)
        {
            navigation.Push(new ChallengeDetailsPage(obj));
        }

        private void ScanQrCode()
        {
            navigation.Push(new QrCodePage());
        }
    }
}