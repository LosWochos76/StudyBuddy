using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private readonly IApi _api;
        private readonly NewsViewModel _newsViewModel;
        private string _createCommentText = "";

        public CommentModalPageViewModel(NewsViewModel viewModel, CollectionView commentCollectionView)
        {
            CommentCollectionView = commentCollectionView;
            _newsViewModel = viewModel;
            _api = TinyIoCContainer.Current.Resolve<IApi>();
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

        public RangeObservableCollection<CommentViewModel> Comments => _newsViewModel.Comments;
        public event PropertyChangedEventHandler PropertyChanged;

        public void CreateComment()
        {
            _api.CommentService.Create(new CommentInsert
            {
                NotificationId = _newsViewModel.Id,
                Text = CreateCommentText
            });

            Comments.Add(new CommentViewModel(new Comment
            {
                Owner = _api.Authentication.CurrentUser,
                Text = CreateCommentText,
                NotificationId = _newsViewModel.Id
            }));

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