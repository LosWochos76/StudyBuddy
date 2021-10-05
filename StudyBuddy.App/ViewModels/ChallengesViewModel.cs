using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengesViewModel : ViewModelBase
    {
        public IEnumerable<Challenge> Challenges { get; private set; } = new List<Challenge>();
        public ICommand RefreshCommand { get; }
        public ICommand DetailsCommand { get; }
        public bool IsRefreshing { get; set; }
        public string Header => string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());
        public string SearchText { get; set; }

        public ChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            RefreshCommand = new Command(Reload);
            DetailsCommand = new Command<Challenge>(ShowDetails);
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            Challenges = await api.Challenges.ForToday(SearchText);
            NotifyPropertyChanged("Challenges");

            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
        }

        private void ShowDetails(Challenge obj)
        {
            navigation.Push(new ChallengeDetailsPage(obj));
        }
    }
}