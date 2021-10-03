using Android.App;
using Android.Content;
using Android.OS;
using Plugin.FirebasePushNotification;

namespace App.Droid.Misc
{
    public class SetupFirebaseAndroid
    {
        public static void SetupPushNotifications(Context context)
        {
            
            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here 
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";

                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
            }

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(context, true);
#else
            FirebasePushNotificationManager.Initialize(context, false);
#endif

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {


            };
        }
        
    }
}