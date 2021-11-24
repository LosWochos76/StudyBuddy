using System.Threading.Tasks;
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

            BindingContext = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
            
        }
    }
}