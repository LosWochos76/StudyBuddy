using StudyBuddy.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    public partial class AcceptedChallengesPage : ContentPage
    {    
        private readonly AcceptedChallengesViewModel view_model;

        public AcceptedChallengesPage()
        {
            InitializeComponent();
            view_model = TinyIoCContainer.Current.Resolve<AcceptedChallengesViewModel>();

            BindingContext = view_model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is StatisticsViewModel vm)
            {
                vm.RefreshCommand.Execute(null);
            }
        }
    }
}