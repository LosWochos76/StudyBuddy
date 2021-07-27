using System;
using StudyBuddy.ServiceFacade;
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
            DependencyService.Register<IAuthentication, LocalAuthentication>();
            DependencyService.Register<IChallengeRepository, LocalChallengeRepository>();

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
