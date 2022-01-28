using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class ThemeViewModel : INotifyPropertyChanged
    {
        private int theme = 0;

        public ThemeViewModel()
        {
            theme = Preferences.Get(nameof(Theme), 0);
            ApplyTheme();
        }

        public int Theme
        {
            get
            {
                return theme;
            }

            set
            {
                theme = value;
                Preferences.Set(nameof(Theme), theme);
                ApplyTheme();
                NotifyPropertyChanged();
            }
        }

        public void ApplyTheme()
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
            get { return Theme == 0; }
            set
            {
                if (value)
                {
                    Theme = 0;
                    NotifyPropertyChanged("IsSystemTheme");
                    NotifyPropertyChanged("IsLightTheme");
                    NotifyPropertyChanged("IsDarkTheme");
                }
            }
        }

        public bool IsLightTheme
        {
            get { return Theme == 1; }
            set
            {
                if (value)
                {
                    Theme = 1;
                    NotifyPropertyChanged("IsSystemTheme");
                    NotifyPropertyChanged("IsLightTheme");
                    NotifyPropertyChanged("IsDarkTheme");
                }
            }
        }

        public bool IsDarkTheme
        {
            get { return Theme == 2; }
            set
            {
                if (value)
                {
                    Theme = 2;
                    NotifyPropertyChanged("IsSystemTheme");
                    NotifyPropertyChanged("IsLightTheme");
                    NotifyPropertyChanged("IsDarkTheme");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 