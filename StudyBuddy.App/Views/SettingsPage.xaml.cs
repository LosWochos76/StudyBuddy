using StudyBuddy.App.Misc;
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
            switch (Settings.Theme)
            {
                case 0:
                    RadioButtonSystem.IsChecked = true;
                    break;
                case 1:
                    RadioButtonLight.IsChecked = true;
                    break;
                case 2:
                    RadioButtonDark.IsChecked = true;
                    break;
            }

            view_model = TinyIoCContainer.Current.Resolve<SettingsViewModel>();
            BindingContext = view_model;
        }

        void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var val = (sender as RadioButton)?.Value as string;
            if (string.IsNullOrWhiteSpace(val))
                return;
            switch (val)
            {
                case "System":
                    Settings.Theme = 0;
                    break;
                case "Light":
                    Settings.Theme = 1;
                    break;
                case "Dark":
                    Settings.Theme = 2;
                    break;
            }

            MainTheme.SetTheme();
        }
    }
}