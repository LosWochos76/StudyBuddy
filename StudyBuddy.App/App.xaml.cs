using System;
using App.Views;
using StudyBuddy.ApiFacade;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Replace the registration of local services by the restful versions to have a real experience
            DependencyService.Register<IAuthentication, RestfulAuthentication>();
            DependencyService.Register<IChallengeRepository, RestfulChallengeRepository>();

            MainPage = new MainPage();
        }
        
        protected async override void OnStart()
        {
            if (!Application.Current.Properties.ContainsKey("Login"))
                return;

            var content = Application.Current.Properties["Login"].ToString();
            var result = await DependencyService.Get<IAuthentication>().LoginFromJson(content);

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
