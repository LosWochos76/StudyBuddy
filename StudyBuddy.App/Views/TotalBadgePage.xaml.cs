using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TotalBadgePage
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