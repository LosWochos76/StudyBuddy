using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.App.Annotations;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class CommentViewModel : INotifyPropertyChanged
    {
        private readonly Comment _comment;

        public CommentViewModel(Comment comment)
        {
            _comment = comment;
        }

        public string Text => _comment.Text;

        public UserViewModel Owner => UserViewModel.FromModel(_comment.Owner);


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}