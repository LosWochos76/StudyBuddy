using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.App.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> AcceptedChallengesCount();
    }
}
