using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class RegisterPage : ContentPage
    {
        private readonly RegisterViewModel view_model;

        public RegisterPage()
        {
            InitializeComponent();
            view_model = TinyIoCContainer.Current.Resolve<RegisterViewModel>();
            BindingContext = view_model;
        }

    }
}