using StudyBuddy.App.Controls;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ChallengeDetailsPage : ContentPage
    {
        private Challenge obj;

        public ChallengeDetailsPage(Challenge obj)
        {
            InitializeComponent();

            this.obj = obj;
            BindingContext = obj;

            BuildUi();
        }

        private void BuildUi()
        {
            if (this.obj.Prove == ChallengeProve.ByQrCode)
            {
                stack.Children.Add(new ByQrCodeView());
            }
        }
    }
}