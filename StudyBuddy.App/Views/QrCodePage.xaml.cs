using StudyBuddy.App.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.Views
{
    public partial class QrCodePage : ContentPage
    {
        private QrCodeViewModel view_model;

        public QrCodePage()
        {
            InitializeComponent();

            view_model = TinyIoCContainer.Current.Resolve<QrCodeViewModel>();
            BindingContext = view_model;
        }

        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                view_model.AcceptFromQrCode(result.Text);
            });
        }
    }
}
