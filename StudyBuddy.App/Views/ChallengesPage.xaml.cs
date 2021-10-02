using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ChallengesPage : ContentPage
    {
        private readonly ChallengesViewModel view_model;

        public ChallengesPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
            BindingContext = view_model;
            view_model.Reload();
        }
    }
}