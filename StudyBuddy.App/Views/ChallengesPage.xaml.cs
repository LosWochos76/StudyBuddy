using StudyBuddy.App.ViewModels;
using Xamarin.Forms;

namespace App.Views
{
    public partial class ChallengesPage : ContentPage
    {
        public ChallengesPage()
        {
            InitializeComponent();

            BindingContext = new ChallengesViewModel();
        }
    }
}
