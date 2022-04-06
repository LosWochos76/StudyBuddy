using StudyBuddy.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsLikedUsersModalPage : ContentPage
    {
        public NotificationsLikedUsersModalPage(NewsViewModel viewModel)
        {
            InitializeComponent();
            var viewmodel = new NotificationsLikedUsersModalPageViewModel(viewModel);
            BindingContext = viewmodel;
        }
    }
}