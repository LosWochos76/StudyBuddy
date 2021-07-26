using System;
using System.Collections.Generic;
using System.Windows.Input;
using StudyBuddy.ServiceFacade;
using Xamarin.Forms;

namespace App
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            Facade.Instance.Authentication.Logout();
        }
    }
}
