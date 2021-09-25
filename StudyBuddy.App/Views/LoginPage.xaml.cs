using Xamarin.Forms;
using StudyBuddy.App.ViewModels;

namespace App.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = new LoginViewModel();
        }
    }
}
