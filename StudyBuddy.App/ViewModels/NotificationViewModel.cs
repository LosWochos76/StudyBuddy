using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using StudyBuddy.App.Api;
using StudyBuddy.App.Views;
using StudyBuddy.Model;
using TinyIoC;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class NotificationViewModel : Notification, INotifyPropertyChanged
    {
        private readonly IApi api;
        public event PropertyChangedEventHandler PropertyChanged;
        public new UserViewModel Owner { get; set; }
        public new IEnumerable<UserViewModel> LikedUsers { get; set; } = new List<UserViewModel>();
        public bool HasBadge => BadgeId.HasValue;
        public new GameBadgeViewModel Badge { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand ShareCommand { get; set; }

        public NotificationViewModel()
        {
            api = TinyIoCContainer.Current.Resolve<IApi>();
            LikeCommand = new AsyncCommand(Like);
            ShareCommand = new AsyncCommand(Share);
        }

        private bool DidILikeIt
        {
            get
            {
                return LikedUsers
                    .ToList()
                    .Any(obj => obj.ID == api.Authentication.CurrentUser.ID);
            }
        }

        public string UsersWhoLikedText
        {
            get
            {
                int count = LikedUsers.Count();
                if (count == 0)
                    return "";

                if (DidILikeIt)
                {
                    if (count > 1)
                        return $"Dir und {count - 1} anderen gefällt das.";
                    else
                        return "Dir gefällt das.";
                }
                else
                {
                    return $"{count} Nutzern gefällt das.";
                }
            }
        }

        public string LikeButtonText
        {
            get
            {
                if (DidILikeIt)
                    return "👍 Like";

                return "👍🏻 Like";
            }
        }

        public object TTinyIoCContainer { get; }

        private void OpenLikedUsers()
        {
            api.Device.PushPage(new NotificationsLikedUsersModalPage(this));
        }

        public async Task Like()
        {
            bool did_i_like_it = DidILikeIt;
            await api.Notifications.Like(this, !DidILikeIt);
            LikedUsers = await api.Users.Likers(Id);

            NotifyPropertyChanged("LikedUsers");
            NotifyPropertyChanged("UsersWhoLikedText");
            NotifyPropertyChanged("LikeButtonText");
        }

        public async Task Share()
        {
            await api.Notifications.Share(this);
            await api.Device.Share(Title, Body);
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}