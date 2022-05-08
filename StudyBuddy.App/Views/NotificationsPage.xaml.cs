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
        private readonly NotificationsPageViewModel ViewModel;

        public NotificationsPage()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            ViewModel = TinyIoCContainer.Current.Resolve<NotificationsPageViewModel>();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NotificationsPageViewModel vm)
                vm.RefreshCommand.Execute(null);


            NotificationCollectionView.Scrolled += (sender, args) =>
            {
                var index = args.FirstVisibleItemIndex;
                var notification = ViewModel.News[index];

                if (notification.Notification.Seen) return;

                ViewModel.Api.NotificationUserMetadataService.SetNotificationToSeen(notification);
            };
        }
    }
}