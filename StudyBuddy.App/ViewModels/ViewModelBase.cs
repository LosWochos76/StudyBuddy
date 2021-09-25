using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.ApiFacade;
using StudyBuddy.App.Misc;

namespace StudyBuddy.App.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected IApiFacade api;
        protected IDialogService dialog;
        protected INagigationService navigation;

        public ViewModelBase()
        {
            LoadServices();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void LoadServices()
        {
            api = App.ServiceProvider.GetService(typeof(IApiFacade)) as IApiFacade;
            dialog = App.ServiceProvider.GetService(typeof(IDialogService)) as IDialogService;
            navigation = App.ServiceProvider.GetService(typeof(INagigationService)) as INagigationService;
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
