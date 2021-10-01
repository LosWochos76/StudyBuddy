using Xamarin.Forms;
using StudyBuddy.App.ViewModels;
using TinyIoC;

namespace StudyBuddy.App.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<LoginViewModel>();
        }
    }
}
