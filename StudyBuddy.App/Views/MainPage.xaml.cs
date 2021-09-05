using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.ApiFacade;
using Xamarin.Forms;

namespace App.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            var authentication = DependencyService.Get<IAuthentication>();
            authentication.Logout();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
