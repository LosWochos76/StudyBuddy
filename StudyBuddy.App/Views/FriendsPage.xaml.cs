using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class FriendsPage : ContentPage
    {
        private readonly FriendsViewModel view_model;
        private readonly IApi api;

        public FriendsPage()
        {
            InitializeComponent();

            api = TinyIoCContainer.Current.Resolve<IApi>();
            view_model = TinyIoCContainer.Current.Resolve<FriendsViewModel>();
            BindingContext = view_model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (api.Authentication.IsLoggedIn)
                view_model.RefreshCommand.Execute(null);
        }
    }
}