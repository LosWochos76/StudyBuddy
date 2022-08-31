using StudyBuddy.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class UserViewModel : User, INotifyPropertyChanged
    {
        public int CountOfCommonFriends => CommonFriends;

        private RequestViewModel request;
        public RequestViewModel FriendshipRequest
        {
            get { return request; }
            set
            {
                request = value;
                NotifyPropertyChanged("FriendshipRequest");
                NotifyPropertyChanged("RequestedForFriendship");
                NotifyPropertyChanged("NotRequestedForFriendship");
                NotifyPropertyChanged("StatusColor");
                NotifyPropertyChanged("StatusText");
            }
        }

        public bool RequestedForFriendship { get { return FriendshipRequest != null; } }

        public string Name
        {
            get
            {
                return $"{Firstname} {Lastname}";
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1} ({2})", Firstname, Lastname, Nickname);
            }
        }

        private ImageSource profile_image = null;
        public ImageSource ProfileImage
        {
            get
            {
                return profile_image;
            }
            set
            {
                profile_image = value;
                NotifyPropertyChanged();
            }
        }

        public string Initials
        {
            get
            {
                return (Firstname.Substring(0, 1) + Lastname.Substring(0, 1)).ToUpper();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static UserViewModel FromModel(User u)
        {
            return new UserViewModel()
            {
                ID = u.ID,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Nickname = u.Nickname,
                Email = u.Email,
                Role = u.Role,
                AccountActive = u.AccountActive,
                EmailConfirmed = u.EmailConfirmed
            };
        }

        public static User ToModel(UserViewModel u)
        {
            return new User()
            {
                ID = u.ID,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Nickname = u.Nickname,
                Email = u.Email,
                Role = u.Role,
                AccountActive = u.AccountActive,
                EmailConfirmed = u.EmailConfirmed
            };
        }

        public Color StatusColor
        {
            get
            {
                if (RequestedForFriendship)
                    return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#E57373") : Color.FromHex("#911a1a");
                else
                    return Application.Current.UserAppTheme == OSAppTheme.Light ? Color.FromHex("#81C784") : Color.FromHex("#327135");
            }
        }

        public string StatusText
        {
            get
            {
                if (RequestedForFriendship)
                    return "Abbrechen";
                else
                    return "Hinzufügen";
            }
        }
    }
}
