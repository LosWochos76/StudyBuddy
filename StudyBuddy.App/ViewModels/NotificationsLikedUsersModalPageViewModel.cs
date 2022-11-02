using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationsLikedUsersModalPageViewModel : INotifyPropertyChanged
    {
        public RangeObservableCollection<UserViewModel> Users { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public NotificationsLikedUsersModalPageViewModel(NotificationViewModel notification)
        {
            Users.AddRange(notification.LikedUsers);
            OnPropertyChanged("Users");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}