using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class UserViewModel : User, INotifyPropertyChanged
    {
        public int CountOfCommonFriends { get; set; }

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
            }
        }

        public bool RequestedForFriendship { get { return FriendshipRequest != null; } }
        public bool NotRequestedForFriendship { get { return FriendshipRequest == null; } }

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
    }
}
