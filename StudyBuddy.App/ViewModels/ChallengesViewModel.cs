using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengesViewModel : ViewModelBase 
    {
        public IEnumerable<Challenge> Challenges { get; private set; } = new List<Challenge>();
        public ICommand RefreshCommand { get; private set; }
        public bool IsRefreshing { get; set; }

        public ChallengesViewModel() : base()
        {
            this.LoadChallenges();
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            RefreshCommand = new Command(LoadChallenges);
        }

        public string Header
        {
            get
            {
                return string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());
            }
        }

        private void Authentication_LoginStateChanged(object sender, ApiFacade.LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                LoadChallenges();
        }

        private async void LoadChallenges()
        {
            IsRefreshing = true;
            NotifyPropertyChanged("IsRefreshing");
            Challenges = await api.Challenges.ForToday("");
            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
            NotifyPropertyChanged("Challenges");
        }
    }
}
