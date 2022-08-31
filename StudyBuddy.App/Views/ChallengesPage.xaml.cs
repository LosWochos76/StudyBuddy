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
        private readonly IApi api;

        public ChallengesPage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<ChallengesViewModel>();
            BindingContext = view_model;
            api = TinyIoCContainer.Current.Resolve<IApi>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (api.Authentication.IsLoggedIn)
                view_model.RefreshCommand.Execute(null);
        }
    }
}