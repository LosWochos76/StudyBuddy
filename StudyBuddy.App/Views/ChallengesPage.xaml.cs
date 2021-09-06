using StudyBuddy.App.Services;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App.Views
{
    public partial class ChallengesPage : ContentPage
    {
        public ChallengesPage()
        {
            InitializeComponent();
            var mockList = new MockDataStore();
            var ChallengesList = mockList.GetMockChallenges();
            ch1.ItemsSource = ChallengesList;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("clicked");
            
        }
    }
}
