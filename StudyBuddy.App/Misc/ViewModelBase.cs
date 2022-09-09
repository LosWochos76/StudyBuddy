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

        public ViewModelBase(IApi api)
        {
            this.api = api;
        }
    }
}