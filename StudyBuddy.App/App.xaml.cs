﻿using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using StudyBuddy.App.Views;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetupServices();

            MainPage = new MainPage();
        }

        private void SetupServices()
        {
            TinyIoCContainer.Current.Register<IApi>(new ApiFacade.ApiFacade());
            TinyIoCContainer.Current.Register<IDialogService>(new DialogService());
            TinyIoCContainer.Current.Register<INavigationService>(new NagigationService());

            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<LoginViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<MainViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<ChallengesViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<QrCodeViewModel>());
            TinyIoCContainer.Current.Register(TinyIoCContainer.Current.Resolve<SettingsViewModel>());
        }

        protected override async void OnStart()
        {
            if (!Current.Properties.ContainsKey("Login"))
                return;

            var content = Current.Properties["Login"].ToString();
            var api = TinyIoCContainer.Current.Resolve<IApi>();
            var result = await api.Authentication.LoginFromJson(content);

            if (result)
                await Shell.Current.GoToAsync("//ChallengesPage");
        }

        protected override void OnSleep()
        {
        }

        protected override async void OnResume()
        {
        }
    }
}