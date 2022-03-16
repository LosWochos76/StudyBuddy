using StudyBuddy.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StudyBuddy.App.ViewModels
{
    public class RankingViewModel
    {
        public ObservableCollection<RankEntry> Ranks { get; set; }

        public RankingViewModel(IEnumerable<RankEntry> ranks)
        {
            this.Ranks = new ObservableCollection<RankEntry>(ranks);
        }
    }
}