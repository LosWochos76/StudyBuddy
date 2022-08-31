using Plugin.Media;
using Plugin.Media.Abstractions;
using StudyBuddy.App.Api;
using StudyBuddy.App.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfileViewModel ViewModel { get; set; }

        public ProfilePage()
        {
            InitializeComponent();

            ViewModel = TinyIoCContainer.Current.Resolve<ProfileViewModel>();
            BindingContext = ViewModel;
        }

        uint duration = 100;
        double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;

        private async void ProfileImage_Tapped(object sender, EventArgs e)
        {
            if (Backdrop.Opacity == 0)
                await OpenDrawer();
            else
                await CloseDrawer();
        }

        private async Task OpenDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(1, length: duration),
                BottomToolbar.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
            );

            Backdrop.InputTransparent = false;
        }

        private async Task CloseDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: duration),
                BottomToolbar.TranslateTo(0, 260, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = true;
        }

        private async void TapGestureRecognizer_Tapped1(System.Object sender, System.EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }

        private async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;

                if (e.TotalY > 0)
                    BottomToolbar.TranslationY = openY + e.TotalY;
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                if (lastPanY < 110)
                    await OpenDrawer();
                else
                    await CloseDrawer();

                isBackdropTapEnabled = true;
            }
        }

        private async void SelectProfileImageFromGalery(object sender, EventArgs e)
        {
            await CloseDrawer();
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Gallerie nicht verfügbar", "StudyBuddy hat keine Berechtigung auf Fotos auf diesem Gerät zuzugreifen", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 500,
                SaveMetaData = true,
            });

            if (file == null)
                return;

            ViewModel.CurrentUser.ProfileImage = ImageSource.FromStream(() => file.GetStream());
 
            var api = TinyIoCContainer.Current.Resolve<IApi>();
            await api.ImageService.SaveProfileImage(ViewModel.CurrentUser, file);
        }

        private async void SelectProfileImageFromCamera(object sender, EventArgs e)
        {
            await CloseDrawer();
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Kamera nicht verfügbar", "StudyBuddy hat keine Berechtigung auf die Kamera zuzugreifen", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 500,
                SaveMetaData = true,
            });

            if (file == null)
                return;

            ViewModel.CurrentUser.ProfileImage = ImageSource.FromStream(() => file.GetStreamWithImageRotatedForExternalStorage());

            var api = TinyIoCContainer.Current.Resolve<IApi>();
            await api.ImageService.SaveProfileImage(ViewModel.CurrentUser, file);
        }

    }
}