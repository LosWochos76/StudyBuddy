using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ChallengesCompletedPage : ContentPage
    {
        private readonly ChallengesCompletedViewModel view_model;
        public ChallengesCompletedPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<ChallengesCompletedViewModel>();
            BindingContext = view_model;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            view_model.Refresh();

            if(BindingContext is StatisticsViewModel vm)
            {
                vm.RefreshCommand.Execute(null);
            }
        }
    }
}