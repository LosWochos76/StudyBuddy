using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Plugin.Media.Abstractions;
using StudyBuddy.App.Misc;
using StudyBuddy.Model;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public int CountOfCommonFriends { get; set; }

        public bool IsAdmin => Role == Role.Admin;
        public bool IsStudent => Role == Role.Student;
        public bool IsInstructor => Role == Role.Instructor;

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

        private PersistentImageViewModel profile_image = null;
        public PersistentImageViewModel Image
        {
            get { return profile_image; }
            set
            {
                profile_image = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ProfileImage");
            }
        }

        public void SetProfileImage(MediaFile file)
        {
            if (profile_image == null)
            {
                profile_image = new PersistentImageViewModel();
                profile_image.UserID = this.ID;
                profile_image.Name = "profile.jpg";
            }

            using (var ms = new MemoryStream())
            {
                file.GetStreamWithImageRotatedForExternalStorage().CopyTo(ms);
                profile_image.Content = ms.ToArray();
                profile_image.Length = profile_image.Content.Length;
            }

            NotifyPropertyChanged("ProfileImage");
        }

        public ImageSource ProfileImage
        {
            get
            {
                if (profile_image != null)
                    return profile_image.ToImageSource();
                else
                    return null;
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
                CountOfCommonFriends = u.CommonFriends
            };
        }

        public bool ContainsAny(string search_text)
        {
            if (string.IsNullOrEmpty(search_text))
                return true;

            var keywords = Helper.SplitIntoKeywords(search_text);
            return Helper.ContainsAny(Firstname + Lastname + Nickname + Email, keywords);
        }
    }
}
