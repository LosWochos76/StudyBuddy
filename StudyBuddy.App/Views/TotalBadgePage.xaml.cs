using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class TotalBadgePage : ContentPage
    {
        private readonly TotalBadgeViewModel view_model;
        public TotalBadgePage()
        {
            InitializeComponent();
            
            view_model = TinyIoCContainer.Current.Resolve<TotalBadgeViewModel>();
            BindingContext = view_model;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            view_model.Refresh();

            if(BindingContext is TotalBadgeViewModel vm)
            {
                vm.RefreshCommand.Execute(null);
            }
        }
    }
}