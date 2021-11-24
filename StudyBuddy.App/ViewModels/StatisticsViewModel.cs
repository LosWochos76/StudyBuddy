using StudyBuddy.App.Api;
using StudyBuddy.App.Misc;
using StudyBuddy.Model.Model;
using System;
using System.Collections.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        public StatisticsViewModel(IApi api, IDialogService dialog, INavigationService navigation) : base(api, dialog, navigation)
        {
            api.Authentication.LoginStateChanged += Authentication_LoginStateChanged;
        }

        private void Authentication_LoginStateChanged(object sender, LoginStateChangedArgs args)
        {
        }
    }
}