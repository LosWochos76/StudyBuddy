using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ThemeViewModel : INotifyPropertyChanged
    {
        private int theme = 0;

        public int Theme
        {
            get
            {
                return Preferences.Get(nameof(Theme), theme);
            }

            set
            {
                theme = value;
                Preferences.Set(nameof(Theme), theme);
                RestoreTheme();
                NotifyPropertyChanged();
            }
        }

        public void RestoreTheme()
        {
            switch (theme)
            {
                case 0:
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                case 1:
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                case 2:
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }
        }

        public bool IsSystemTheme
        {
            get
            {
                return Theme == 0;
            }
            set
            {
                Theme = 0;
            }
        }

        public bool IsLightTheme
        {
            get
            {
                return Theme == 1;
            }
            set
            {
                Theme = 1;
            }
        }

        public bool IsDarkTheme
        {
            get
            {
                return Theme == 2;
            }
            set
            {
                Theme = 2;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}