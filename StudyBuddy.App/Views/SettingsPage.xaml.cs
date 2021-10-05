using StudyBuddy.ApiFacade;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel view_model;

        public SettingsPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<SettingsViewModel>();
            BindingContext = view_model;
        }
    }
}