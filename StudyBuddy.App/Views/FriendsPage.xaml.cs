using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class FriendsPage : ContentPage
    {
        private Task task;
        private FriendsViewModel view_model;

        public FriendsPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<FriendsViewModel>();
            BindingContext = view_model;
        }

        private void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            view_model.SearchText = e.NewTextValue;
            if (task == null || task.IsCompleted)
            {
                task = Task.Run(async () =>
                {
                    await Task.Delay(800);
                    view_model.ApplyFilter();
                });
            }
        }
    }
}
