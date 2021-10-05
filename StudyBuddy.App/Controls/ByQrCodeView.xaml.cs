using System;
using System.Collections.Generic;
using StudyBuddy.App.Misc;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Controls
{
    public partial class ByQrCodeView : ContentView
    {
        public ByQrCodeView()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var navigation = TinyIoCContainer.Current.Resolve<INavigationService>();
            navigation.GoTo("//QrCodePage");
        }
    }
}
