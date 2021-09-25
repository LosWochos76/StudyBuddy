using System;
using App.Views;
using Microsoft.Extensions.DependencyInjection;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;
using Xamarin.Forms;

namespace StudyBuddy.App
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public App()
        {
            InitializeComponent();
            SetupServices();

            MainPage = new MainPage();
        }

        private void SetupServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IApiFacade, ApiFacade.ApiFacade>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INagigationService, NagigationService>();
            ServiceProvider = services.BuildServiceProvider();
        }

        protected async override void OnStart()
        {
            if (!Current.Properties.ContainsKey("Login"))
                return;

            var content = Current.Properties["Login"].ToString();
            var api = ServiceProvider.GetService(typeof(IApiFacade)) as IApiFacade;
            var result = await api.Authentication.LoginFromJson(content);

            if (result)
                await Shell.Current.GoToAsync("//ChallengesPage");
        }

        protected override void OnSleep()
        {
        }

        protected async override void OnResume()
        {
        }
    }
}