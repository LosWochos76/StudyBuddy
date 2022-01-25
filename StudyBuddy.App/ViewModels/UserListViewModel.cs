using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class UserListViewModel : UserList
    {
        public new IEnumerable<UserViewModel> Objects { get; set; }
    }
}
