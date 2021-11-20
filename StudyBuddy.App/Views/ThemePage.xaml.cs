using System;
using System.Collections.Generic;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ThemePage : ContentPage
    {
        private readonly ThemeViewModel view_model;

        public ThemePage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<ThemeViewModel>();
            BindingContext = view_model;
        }
    }
}
