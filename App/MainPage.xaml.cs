using System;
using System.Collections.Generic;
using StudyBuddy.ServiceFacade;
using Xamarin.Forms;

namespace App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Facade.Instance.Authentication.Logout();
        }
    }
}
