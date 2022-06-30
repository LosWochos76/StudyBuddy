using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChallengesPage
    {
        private readonly ChallengesViewModel view_model;

        public ChallengesPage()
        {
            InitializeComponent();
            BindingContext = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ChallengesViewModel cvm)
                cvm.RefreshCommand.Execute(null);
            if (BindingContext is StatisticsViewModel vm)
                vm.RefreshCommand.Execute(null);
        }
    }
}