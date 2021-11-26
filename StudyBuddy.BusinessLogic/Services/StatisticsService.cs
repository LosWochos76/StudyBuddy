using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.Model.Model;
using System;

namespace StudyBuddy.BusinessLogic.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IBackend backend;
                
        public StatisticsService (IBackend backend)
        {
            this.backend = backend;
        }

        public UserStatistics GetUserStatistics(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return backend.Repository.StatisticsRepository.GetUserStatistics(user_id);
        }
    }
}