using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticPage
    {
        private readonly StatisticsViewModel view_model;

        public StatisticPage()
        {
            InitializeComponent();
            BindingContext = TinyIoCContainer.Current.Resolve<StatisticsViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(BindingContext is StatisticsViewModel vm)
                vm.RefreshCommand.Execute(null);
        }
    }
}