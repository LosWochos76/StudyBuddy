﻿using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.App.Models;
using StudyBuddy.App.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {

        private UserStatistics _userStatistic;
        public UserStatistics UserStatistics
        {
            get { return _userStatistic; }
            set
            {
                _userStatistic = value;
                NotifyPropertyChanged("UserStatistics");
            }
        }
        public ICommand RanksCommand { get; set; }


        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            this.RanksCommand = new Command(ShowRanks);
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        private async void ShowRanks()
        {
            await navigation.Push(new RankingPage(UserStatistics.FriendsRank));
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {

        }

        public async void Refresh()
        {
            try
            {
               UserStatistics = await api.Statistics.GetUserStatistics();

            }
            catch (System.Exception)
            {
                await App.Current.MainPage.DisplayAlert("Fehler", "Fehler beim Laden der Statistiken. API Endpunkt nicht erreichbar", "Ok");
            }
        }
    }
}