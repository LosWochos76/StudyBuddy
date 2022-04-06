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
        public NotificationsPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            BindingContext = TinyIoCContainer.Current.Resolve<NotificationsPageViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NotificationsPageViewModel vm)
                vm.RefreshCommand.Execute(null);
        }
    }
}