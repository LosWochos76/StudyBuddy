using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class NewsViewModel : Notification , INotifyPropertyChanged
    {
        public static NewsViewModel FromNotification(Notification notification)
        {
            return new NewsViewModel()
            {
                Id = notification.Id,
                OwnerId = notification.OwnerId,
                Owner = notification.Owner,
                Title = notification.Title,
                Body = notification.Body,
                Created = notification.Created,
                Updated = notification.Updated,
            };
        }
        public string FormattedBody { get => Body + " am " + Created.ToShortDateString(); }
        public UserViewModel NotificationOwner { get => UserViewModel.FromModel(Owner); }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}