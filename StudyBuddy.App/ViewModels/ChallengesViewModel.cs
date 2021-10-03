using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengesViewModel : ViewModelBase
    {
        public ChallengesViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog,
            navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
            RefreshCommand = new Command(Reload);
            AcceptCommand = new Command<Challenge>(Accept);
        }

        public IEnumerable<Challenge> Challenges { get; private set; } = new List<Challenge>();
        public ICommand RefreshCommand { get; }
        public ICommand AcceptCommand { get; }
        public bool IsRefreshing { get; set; }

        public string Header => string.Format("Herausforderungen am {0}", DateTime.Now.ToShortDateString());

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
            if (args.IsLoggedIn)
                Reload();
        }

        public async void Reload()
        {
            IsRefreshing = true;
            NotifyPropertyChanged("IsRefreshing");
            Challenges = await api.Challenges.ForToday("");
            IsRefreshing = false;
            NotifyPropertyChanged("IsRefreshing");
            NotifyPropertyChanged("Challenges");
        }

        private void Accept(Challenge obj)
        {
            dialog.ShowMessageBox(obj.Name, "Herausforderung annehmen");
        }
    }
}