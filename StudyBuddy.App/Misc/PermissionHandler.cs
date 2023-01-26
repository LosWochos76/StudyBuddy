using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StudyBuddy.App.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.Misc
{
    public class PermissionHandler : IPermissionHandler
    {

        public async Task<bool> CheckCameraPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status == PermissionStatus.Denied)
                {
                    await Application.Current.MainPage.DisplayAlert("Kamera nicht verfügbar", "StudyBuddy hat keine Berechtigung auf die Kamera zuzugreifen." +
                        "App-Berechtigungen können in den Geräteeinstellungen verwaltet werden. ", "OK");
                    return false;
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Kamera nicht verfügbar", "StudyBuddy hat keine Berechtigung auf die Kamera zuzugreifen." +
                        "App-Berechtigungen können in den Geräteeinstellungen verwaltet werden. ", "OK");
                return false;
            }
            return true;
        }

        public async Task<bool> CheckStoragePermission()
        {
            try
            {
                var statusStorageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                var statusStorageWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

                if (statusStorageRead == PermissionStatus.Denied || statusStorageWrite == PermissionStatus.Denied)
                {
                    await Application.Current.MainPage.DisplayAlert("Galerie nicht verfügbar", "StudyBuddy hat keine Berechtigung auf den Gerätespeicher zuzugreifen." +
                        "App-Berechtigungen können in den Geräteeinstellungen verwaltet werden. ", "OK");
                    return false;
                }

            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Galerie nicht verfügbar", "StudyBuddy hat keine Berechtigung auf den Gerätespeicher zuzugreifen." +
                    "App-Berechtigungen können in den Geräteeinstellungen verwaltet werden. ", "OK");
                return false;
            }
            return true;
        }
    }
}
