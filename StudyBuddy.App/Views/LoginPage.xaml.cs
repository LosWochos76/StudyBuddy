using Xamarin.Forms;
using StudyBuddy.App.ViewModels;
using TinyIoC;

namespace StudyBuddy.App.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel view_model;

        public LoginPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<LoginViewModel>();
            BindingContext = view_model;
        }
    }
}
