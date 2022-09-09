using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        private readonly IApi api;
        private readonly NotificationsPageViewModel view_model;

        public NotificationsPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);

            this.api = TinyIoCContainer.Current.Resolve<IApi>();
            this.view_model = TinyIoCContainer.Current.Resolve<NotificationsPageViewModel>();
            BindingContext = view_model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (api.Authentication.IsLoggedIn)
                view_model.RefreshCommand.Execute(null);
        }
    }
}