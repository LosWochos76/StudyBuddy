using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ChallengeDetailsPage : ContentPage
    {
        private IApi api;
        private ChallengeViewModel challenge;

        public ChallengeDetailsPage(ChallengeViewModel challenge)
        {
            InitializeComponent();

            this.api = TinyIoCContainer.Current.Resolve<IApi>();
            this.challenge = challenge;
            grid.BindingContext = challenge;

            var ccvm = new ChallengeConfirmViewModel(api, challenge);
            button1.BindingContext = ccvm;
            FriendsPicker.BindingContext = ccvm;

            if (challenge.ProveText != "Durch Bestätigung eines/einer Freundes/Freundin")
                FriendsPicker.IsVisible = false;
        }
    }
}