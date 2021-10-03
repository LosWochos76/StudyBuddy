using System;
using Android.App;
using Android.Runtime;
using App.Droid.Misc;

namespace StudyBuddy.App.Droid
{
    // Registers the class as the main application class. some decorator magic
    // before activity is mounted
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            
            SetupFirebaseAndroid.SetupPushNotifications(this);

 
        }
    }
}