using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class FlyoutHeaderView : ContentView
    {
        public FlyoutHeaderView()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<FlyoutHeaderViewModel>();
        }
    }
}