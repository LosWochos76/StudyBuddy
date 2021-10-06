using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengesViewModel : ViewModelBase
    {
        public IEnumerable<ChallengeViewModel> Challenges { get; private set; } = new List<ChallengeViewModel>();
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public bool IsRefreshing { get; set; }
        public string Header => string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());
        public string SearchText { get; set; }

        public ChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            RefreshCommand = new Command(Reload);
            DetailsCommand = new Command<ChallengeViewModel>(ShowDetails);
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            Challenges = await api.Challenges.ForToday(SearchText, true);
            NotifyPropertyChanged("Challenges");

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        public async void ApplyFilter()
        {
            Challenges = await api.Challenges.ForToday(SearchText, false);
            NotifyPropertyChanged("Challenges");
        }

        private void ShowDetails(ChallengeViewModel obj)
        {
            navigation.Push(new ChallengeDetailsPage(obj));
        }
    }
}