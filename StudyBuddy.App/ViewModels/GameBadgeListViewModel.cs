using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class GameBadgeListViewModel : GameBadgeList
    {
        public new IEnumerable<GameBadgeViewModel> Objects { get; set; }
    }
}