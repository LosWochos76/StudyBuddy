using Plugin.Media;
using Plugin.Media.Abstractions;
using StudyBuddy.App.Api;
using StudyBuddy.App.Interfaces;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfileViewModel ViewModel { get; set; }

        readonly IPermissionHandler _permissionHandler;

        public ProfilePage()
        {
            InitializeComponent();
            _permissionHandler = TinyIoCContainer.Current.Resolve<IPermissionHandler>();
            ViewModel = TinyIoCContainer.Current.Resolve<ProfileViewModel>();
            BindingContext = ViewModel;
        }

        readonly uint _duration = 100;
        readonly double _openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double _lastPanY = 0;
        bool _isBackdropTapEnabled = true;

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
                Backdrop.FadeTo(1, length: _duration),
                BottomToolbar.TranslateTo(0, _openY, length: _duration, easing: Easing.SinIn)
            );

            Backdrop.InputTransparent = false;
        }

        private async Task CloseDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: _duration),
                BottomToolbar.TranslateTo(0, 260, length: _duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = true;
        }

        private async void TapGestureRecognizer_Tapped1(System.Object sender, System.EventArgs e)
        {
            if (_isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }

        private async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                _isBackdropTapEnabled = false;
                _lastPanY = e.TotalY;

                if (e.TotalY > 0)
                    BottomToolbar.TranslationY = _openY + e.TotalY;
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                if (_lastPanY < 110)
                    await OpenDrawer();
                else
                    await CloseDrawer();

                _isBackdropTapEnabled = true;
            }
        }

        private async void SelectProfileImageFromGalery(object sender, EventArgs e)
        {
            await CloseDrawer();
            await CrossMedia.Current.Initialize();

            if(!await _permissionHandler.CheckStoragePermission())
            {
                return;
            }

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

            if (!await _permissionHandler.CheckCameraPermission())
            {
                return;
            }

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