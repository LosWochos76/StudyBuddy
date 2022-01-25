using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class RequestListViewModel : RequestList
    {
        public new IEnumerable<RequestViewModel> Objects { get; set; }
    }
}
