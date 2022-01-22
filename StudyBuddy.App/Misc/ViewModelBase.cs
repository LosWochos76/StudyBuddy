using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected IApi api;
        protected IDialogService dialog;
        private INavigationService navigation;

        protected INavigationService Navigation { get => navigation; set => navigation = value; }

        public ViewModelBase(IApi api, IDialogService dialog, INavigationService navigation)
        {
            this.api = api;
            this.dialog = dialog;
            this.Navigation = navigation;
        }
    }
}