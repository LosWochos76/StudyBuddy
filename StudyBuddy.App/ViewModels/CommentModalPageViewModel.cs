using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using StudyBuddy.App.Annotations;
using StudyBuddy.App.Api;
using StudyBuddy.Model;
using TinyIoC;
using Xamarin.Forms;

namespace StudyBuddy.App.ViewModels
{
    public class CommentModalPageViewModel : INotifyPropertyChanged
    {
        private readonly IApi api;
        private readonly NotificationViewModel notification;
        private string _createCommentText = "";
        private RangeObservableCollection<CommentViewModel> comments;

        public CommentModalPageViewModel(IApi api, NotificationViewModel notification, CollectionView commentCollectionView)
        {
            this.api = TinyIoCContainer.Current.Resolve<IApi>();
            this.notification = notification;
            CommentCollectionView = commentCollectionView;
            comments = new RangeObservableCollection<CommentViewModel>();
            comments.AddRange(notification.Comments.ToList());

            CreateCommentCommand = new Command(CreateComment);
        }

        public CollectionView CommentCollectionView { get; set; }
        public Command CreateCommentCommand { get; set; }

        public string CreateCommentText
        {
            get => _createCommentText;
            set
            {
                _createCommentText = value;
                OnPropertyChanged();
            }
        }

        public RangeObservableCollection<CommentViewModel> Comments => comments;
        public event PropertyChangedEventHandler PropertyChanged;

        public async void CreateComment()
        {
            var comment = await api.Notifications.AddComment(notification, CreateCommentText);
            Comments.Add(comment);
            CommentCollectionView.ScrollTo(Comments.Count - 1);
            CreateCommentText = "";
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}