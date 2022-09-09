using StudyBuddy.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsLikedUsersModalPage : ContentPage
    {
        public NotificationsLikedUsersModalPage(NotificationViewModel notification)
        {
            InitializeComponent();

            Title = "Likes";
            var viewmodel = new NotificationsLikedUsersModalPageViewModel(notification);
            BindingContext = viewmodel;
        }
    }
}