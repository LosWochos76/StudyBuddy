using StudyBuddy.App.ViewModels;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class FriendPage : ContentPage
    {

        public FriendPage(UserViewModel obj)
        {
            InitializeComponent();

            BindingContext = new FriendViewModel(obj);
        }
    }
}
