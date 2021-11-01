using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengesViewModel : ViewModelBase
    {
        public ObservableCollection<ChallengeViewModel> Challenges { get; private set; } = new ObservableCollection<ChallengeViewModel>();
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public ICommand ScanQrCodeCommand { get; }
        public bool IsRefreshing { get; set; }
        public string Header => string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());
        public string SearchText { get; set; }

        public ChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            RefreshCommand = new Command(Reload);
            DetailsCommand = new Command<ChallengeViewModel>(ShowDetails);
            ScanQrCodeCommand = new Command(ScanQrCode);
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            if (!IsRefreshing)
            {
                IsRefreshing = true;
                NotifyPropertyChanged("IsRefreshing");
            }

            try
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    api.Challenges.ForToday(Challenges, SearchText);
                });
            }
            catch (ApiException e)
            {
                await dialog.ShowError(e, "Ein Fehler ist aufgetreten!", "Ok", null);
            }

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        public async void ApplyFilter()
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                api.Challenges.ForToday(Challenges, SearchText);
                NotifyPropertyChanged("Challenges");
            });
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