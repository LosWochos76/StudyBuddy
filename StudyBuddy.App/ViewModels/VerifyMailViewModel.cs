using StudyBuddy.App.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class VerifyMailViewModel : ViewModelBase
    {
        public IAsyncCommand ConfirmCommand { get; }
        public IAsyncCommand CancelCommand { get; }
        public string Email { get; set; }


        public VerifyMailViewModel(IApi api) : base(api)
        {
            ConfirmCommand = new AsyncCommand(ResendMail, () =>
            {
                return IsEmailValid;
            });

            CancelCommand = new AsyncCommand(Cancel);
        }
        private bool is_email_valid = false;
        public bool IsEmailValid
        {
            get { return is_email_valid; }
            set
            {
                is_email_valid = value;
                NotifyPropertyChanged(nameof(IsEmailValid));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }
        public async Task ResendMail()
        {
            if(!Email.ToLower().EndsWith("@hshl.de") && !Email.ToLower().EndsWith("@stud.hshl.de"))
            {
                api.Device.ShowError("Aktuell können nur E-Mail-Adressen der Hochschule Hamm-Lippstadt genutzt werden!", "Fehler!", "Ok", null);
                return;
            }

            var result = api.Authentication.SendVerificationMail(Email);
            if(result == null)
            {
                api.Device.ShowError("E-Mail konnte nicht angefordert werden! Bitte versuchen Sie es später noch einmal!", "Fehler!", "Ok", null);
                return;
            }
            api.Device.ShowMessage("E-Mail versendet! Bitte klicke auf den Link in der Bestätigungsmail, die du erhalten hast. " +
                "Danach kannst du dich in der App einloggen!", "Herzlich willkommen bei Gameucation!");
            await api.Device.PopPage();
        }

        public async Task Cancel()
        {
            await api.Device.PopPage();
        }

    }
}
