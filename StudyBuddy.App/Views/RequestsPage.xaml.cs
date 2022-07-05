using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestsPage
    {
        public RequestsPage()
        {
            InitializeComponent();
            BindingContext = TinyIoCContainer.Current.Resolve<RequestsViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is RequestsViewModel rvm)
            {
                rvm.RefreshCommand.Execute(null);
            }
        }
    }
}