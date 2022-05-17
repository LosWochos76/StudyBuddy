using StudyBuddy.App.ViewModels;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class BadgeDetailsPage : ContentPage
    {
        private GameBadgeViewModel badge;

        public BadgeDetailsPage(GameBadgeViewModel badge)
        {
            InitializeComponent();
            this.badge = badge;
            grid.BindingContext = badge;
        }
    }
}