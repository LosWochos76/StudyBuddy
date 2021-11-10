using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel view_model;

        public LoginPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<LoginViewModel>();
            BindingContext = view_model;
        }

    }
}