using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class TotalBadgePage : ContentPage
    {
        
        public TotalBadgePage()
        {
            InitializeComponent();
            BindingContext = TinyIoCContainer.Current.Resolve<GameBadgesViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is GameBadgesViewModel vm)
                vm.RefreshCommand.Execute(null);
        }


    }
}