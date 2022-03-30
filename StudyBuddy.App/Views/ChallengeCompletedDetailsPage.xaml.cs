using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Views
{
    public partial class ChallengeCompletedDetailsPage
    {
        private ChallengeViewModel challenge;

        public ChallengeCompletedDetailsPage(ChallengeViewModel challenge)
        {
            InitializeComponent();
            this.challenge = challenge;
            grid.BindingContext = challenge;
        }
    }
}