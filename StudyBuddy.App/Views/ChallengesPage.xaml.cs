using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChallengesPage
    {
        private readonly ChallengesViewModel view_model;
        private readonly MainViewModel request_model;
        private readonly IApi api;

        public ChallengesPage()
        {
            InitializeComponent();

            api = TinyIoCContainer.Current.Resolve<IApi>();
            view_model = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
            request_model = TinyIoCContainer.Current.Resolve<MainViewModel>();

            BindingContext = view_model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (api.Authentication.IsLoggedIn)
            {
                view_model.RefreshCommand.Execute(null);
                request_model.GetRequestCountCommand.ExecuteAsync();
            }
                
        }
    }
}