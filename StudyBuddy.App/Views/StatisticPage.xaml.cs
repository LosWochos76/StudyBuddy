using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticPage
    {
        private readonly StatisticsViewModel view_model;
        private readonly int user_id;
        private readonly IApi _api;

        public StatisticPage()
        {
            InitializeComponent();
            _api = TinyIoCContainer.Current.Resolve<IApi>();
            user_id = _api.Authentication.CurrentUser.ID;
            BindingContext = TinyIoCContainer.Current.Resolve<StatisticsViewModel>();
        }

        public StatisticPage(UserViewModel obj)
        {
            InitializeComponent();
            Shell.SetTabBarIsVisible(statistics_page, false);
            user_id = obj.ID;
            BindingContext = TinyIoCContainer.Current.Resolve<StatisticsViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is StatisticsViewModel vm)
                vm.RefreshCommand.Execute(user_id);
        }

    }
}