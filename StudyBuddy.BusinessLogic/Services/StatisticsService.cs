using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.Model;
using System;
using System.Collections.Generic;

namespace StudyBuddy.BusinessLogic.Services
{
    class StatisticsService : IStatisticsService
    {
        private readonly IBackend backend;
                
        public StatisticsService (IBackend backend)
        {
            this.backend = backend;
        }

        public UserStatistics GetUserStatistics(int user_id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.StatisticsRepository.GetUserStatistics(user_id);
        }

        public IEnumerable<User> GetUsersWithDateCreated(bool orderAscending)
        {
            return backend.Repository.StatisticsRepository.GetUsersWithDateCreated(orderAscending);
        }
        public IEnumerable<Challenge> GetChallengeStatistic(bool orderAscending)
        {
            return backend.Repository.StatisticsRepository.GetChallengeStatistic(orderAscending);
        }

        public IEnumerable<GameBadge> GetBadgeStatistics(bool orderAscending)
        {
            return backend.Repository.StatisticsRepository.GetBadgeStatistic(orderAscending);
        }

        public IEnumerable<GameObjectStatistics> GetChallengeCompletionStatistics(bool orderAscending)
        {
            return backend.Repository.StatisticsRepository.GetChallengeCompletionStatistics(orderAscending);
        }
        public IEnumerable<GameObjectStatistics> GetBadgeCompletionStatistics(bool orderAscending)
        {
            return backend.Repository.StatisticsRepository.GetBadgeCompletionStatistics(orderAscending);
        }
    }
}