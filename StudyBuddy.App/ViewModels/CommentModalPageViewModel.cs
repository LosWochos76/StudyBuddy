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


        public CommentModalPageViewModel(NewsViewModel viewModel)
        {
            _newsViewModel = viewModel;
            _api = TinyIoCContainer.Current.Resolve<IApi>();
            Comments = viewModel.Comments;
            CreateCommentCommand = new Command(CreateComment);
        }

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


        public RangeObservableCollection<CommentViewModel> Comments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public void CreateComment()
        {
            _api.CommentService.Create(new CommentInsert
            {
                NotificationId = _newsViewModel.Id,
                Text = CreateCommentText
            });

            Comments.Insert(0, new CommentViewModel(new Comment
            {
                Owner = _api.Authentication.CurrentUser,
                Text = CreateCommentText
            }));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}