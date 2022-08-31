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

            var ccvm = new ChallengeConfirmViewModel(challenge);
            this.challenge = challenge;
            grid.BindingContext = challenge;
            button1.BindingContext = ccvm;
            FriendsPicker.BindingContext = ccvm;
            //FriendsPicker.SelectedItem 
            if (challenge.ProveText != "Durch Bestätigung eines/einer Freundes/Freundin")
                FriendsPicker.IsVisible = false;
        }
    }
}