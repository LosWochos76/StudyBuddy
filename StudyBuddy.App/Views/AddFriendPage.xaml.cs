using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class AddFriendPage : ContentPage
    {
        public AddFriendPage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<AddFriendViewModel>();
        }
    }
}