using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {

                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
                {
                    Title = "Wähle ein neues Profilbild aus."
                });

                if (result == null)
                    return;

                var stream = await result.OpenReadAsync();
                image.Source = ImageSource.FromStream(() => stream);
            }
            catch (Exception)
            {

            }
        }

        private async void Button_Clicked1(System.Object sender, System.EventArgs e)
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync();

                if (result == null)
                    return;

                var stream = await result.OpenReadAsync();

                image.Source = ImageSource.FromStream(() => stream);
                await image.RotateTo(90);
            }
            catch (Exception)
            {

            }
        }
    }
}
