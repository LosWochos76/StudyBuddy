﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class FriendsPage : ContentPage
    {
        public FriendsPage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<FriendsViewModel>();
        }
    }
}
