using System;
using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationsPage : ContentPage
    {
        
        
        
        public NotificationsPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS> ().SetUseSafeArea(true);
            BindingContext = TinyIoCContainer.Current.Resolve<NotificationsViewModel>();
            
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NotificationsViewModel vm)
                vm.RefreshCommand.Execute(null);

       
        }
        
    }
}