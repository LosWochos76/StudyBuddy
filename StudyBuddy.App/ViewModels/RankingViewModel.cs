using StudyBuddy.Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
