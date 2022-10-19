using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyEmailPage : ContentPage
    {
        private readonly VerifyMailViewModel view_model;
        public VerifyEmailPage()
        {
            InitializeComponent();
            view_model = TinyIoCContainer.Current.Resolve<VerifyMailViewModel>();
            BindingContext = view_model;
        }
    }
}