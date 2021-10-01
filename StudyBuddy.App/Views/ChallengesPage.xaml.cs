using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ChallengesPage : ContentPage
    {
        public ChallengesPage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
        }
    }
}
