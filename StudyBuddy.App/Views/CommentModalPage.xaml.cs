using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommentModalPage : ContentPage
    {
        private readonly IApi api;

        public CommentModalPage(NotificationViewModel notification)
        {
            InitializeComponent();

            api = TinyIoCContainer.Current.Resolve<IApi>();
            Title = "Kommentare"; 
            BindingContext = new CommentModalPageViewModel(api, notification, CommentCollectionView);
        }
    }
}