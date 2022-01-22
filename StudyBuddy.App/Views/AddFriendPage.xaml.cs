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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is AddFriendViewModel vm)
                vm.RefreshCommand.Execute(null);
        }
    }
}