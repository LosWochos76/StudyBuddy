﻿using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<MainViewModel>();
        }
    }
}
