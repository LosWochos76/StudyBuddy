using System;
using System.Collections.Generic;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ThemePage : ContentPage
    {
        public ThemePage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<ThemeViewModel>();
        }
    }
}
