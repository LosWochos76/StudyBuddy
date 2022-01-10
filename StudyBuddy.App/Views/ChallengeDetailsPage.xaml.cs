using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ChallengeDetailsPage : ContentPage
    {
        private ChallengeViewModel challenge;

        public ChallengeDetailsPage(ChallengeViewModel challenge)
        {
            InitializeComponent();
            this.challenge = challenge;
            grid.BindingContext = challenge;
            stack.BindingContext = new ChallengeConfirmViewModel(challenge);

        }

        
    }
}