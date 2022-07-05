using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage
    {
        private readonly MainViewModel view_model;

        public MainPage()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddFriendPage), typeof(AddFriendPage));
            Routing.RegisterRoute(nameof(QrCodePage), typeof(QrCodePage));
            BindingContext = TinyIoCContainer.Current.Resolve<MainViewModel>();
        }
    }
}