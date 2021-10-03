using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class MainPage : Shell
    {
        private readonly MainViewModel view_model;

        public MainPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<MainViewModel>();
            BindingContext = view_model;
        }
    }
}