using Plugin.Media;
using Plugin.Media.Abstractions;
using StudyBuddy.App.Misc;
using StudyBuddy.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudyBuddy.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {

        public ProfilePage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<FlyoutHeaderViewModel>();
        }
        uint duration = 100;
        double openY = (Device.RuntimePlatform == "Android") ? 20 : 60;
        double lastPanY = 0;
        bool isBackdropTapEnabled = true;
        async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (Backdrop.Opacity == 0)
            {
                await OpenDrawer();
            }
            else
            {
                await CloseDrawer();
            }
        }

        async Task OpenDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(1, length: duration),
                BottomToolbar.TranslateTo(0, openY, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = false;
        }

        async Task CloseDrawer()
        {
            await Task.WhenAll
            (
                Backdrop.FadeTo(0, length: duration),
                BottomToolbar.TranslateTo(0, 260, length: duration, easing: Easing.SinIn)
            );
            Backdrop.InputTransparent = true;
        }
        async void TapGestureRecognizer_Tapped1(System.Object sender, System.EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await CloseDrawer();
            }
        }

        async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;
                if (e.TotalY > 0)
                {
                    BottomToolbar.TranslationY = openY + e.TotalY;
                }

            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                if (lastPanY < 110)
                {
                    await OpenDrawer();
                }
                else
                {
                    await CloseDrawer();
                }
                isBackdropTapEnabled = true;
            }
        }

        private async void TextCell_Tapped(object sender, EventArgs e)
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
                PhotoSize = PhotoSize.Full,
                SaveMetaData = true
            });
            if (file == null)
                return;
            image1.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

        }

        private async void TextCell_Tapped_1(object sender, EventArgs e)
        {
            await CloseDrawer();
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Kamera nicht verfügbar", "StudyBuddy hat keine Berechtigung auf die Kamera zuzugreifen", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                SaveMetaData = true
            });
            if (file == null)
                return;
            var storableImage = ImageToByteA(file);

        }

        public byte[] ImageToByteA(MediaFile image)
        {
            image1.Source = ImageSource.FromStream(() =>
            {
                var stream = image.GetStream();
                return stream;
            });
            using (var memoryStream = new MemoryStream())
            {
                image.GetStream().CopyTo(memoryStream);
                image.Dispose();
                return memoryStream.ToArray();
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}