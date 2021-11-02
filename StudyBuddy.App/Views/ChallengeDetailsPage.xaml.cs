using StudyBuddy.App.Controls;
using StudyBuddy.App.ViewModels;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ChallengeDetailsPage : ContentPage
    {
        private ChallengeViewModel obj;

        public ChallengeDetailsPage(ChallengeViewModel obj)
        {
            InitializeComponent();

            this.obj = obj;
            grid.BindingContext = obj;
            stack.BindingContext = new ChallengeConfirmViewModel((int)obj.Prove, (int)obj.ID);

            //BuildUi();
        }

        /* private void BuildUi()
        {
            if (this.obj.Prove == ChallengeProve.ByQrCode)
                stack.Children.Add(new ByQrCodeView());

            if (this.obj.Prove == ChallengeProve.ByRandomTeamMember)
                stack.Children.Add(new ByFriendView(obj));
        }*/
    }
}