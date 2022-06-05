using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class ChallengeListViewModel : ChallengeList
    {
        public new IEnumerable<ChallengeViewModel> Objects { get; set; }
    }
}