using StudyBuddy.App.Api;
using StudyBuddy.App.Models;
using StudyBuddy.App.ViewModels;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class FriendPage : ContentPage
    {
        private readonly FriendViewModel view_model;

        public FriendPage(UserViewModel obj, UserStatistics userStatistics)
        {
            InitializeComponent();

            view_model = new FriendViewModel(obj, userStatistics);
            BindingContext = view_model;
        }

    }
}
