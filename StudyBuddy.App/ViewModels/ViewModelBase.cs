using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected IApi api;
        protected IDialogService dialog;
        protected INavigationService navigation;

        public ViewModelBase(IApi api, IDialogService dialog, INavigationService navigation)
        {
            this.api = api;
            this.dialog = dialog;
            this.navigation = navigation;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}