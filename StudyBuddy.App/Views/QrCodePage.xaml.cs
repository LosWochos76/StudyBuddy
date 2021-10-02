using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using ZXing;

namespace StudyBuddy.App.Views
{
    public partial class QrCodePage : ContentPage
    {
        private readonly QrCodeViewModel view_model;

        public QrCodePage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<QrCodeViewModel>();
            BindingContext = view_model;
        }

        private void ZXingScannerView_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(() => { view_model.AcceptFromQrCode(result.Text); });
        }
    }
}