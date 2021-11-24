using StudyBuddy.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.BusinessLogic.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IBackend backend;
                
        public StatisticsService (IBackend backend)
        {
            this.backend = backend;
        }

        public int GetAcceptedChallengesCount(int user_id)
        {
            //if (backend.CurrentUser == null)
            //    throw new Exception("Unauthorized");

            return backend.Repository.Statistics.AcceptedChallengesCountForUser(user_id);
        }
    }
}
