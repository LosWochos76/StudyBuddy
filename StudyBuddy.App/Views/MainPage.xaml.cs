using StudyBuddy.App.ViewModels;
using Xamarin.Forms;

namespace App.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel();
        }
    }
}
