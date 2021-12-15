﻿using StudyBuddy.BusinessLogic.Interfaces;
using StudyBuddy.Model.Model;
using System;
using System.Collections.Generic;

namespace StudyBuddy.BusinessLogic.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private readonly IBackend backend;
                
        public StatisticsService (IBackend backend)
        {
            this.backend = backend;
        }

        public UserStatistics GetUserStatistics(int user_id)
        {
            // Wieso soll man Admin sein, um seine Statsitiken zu sehen?!?
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.StatisticsRepository.GetUserStatistics(user_id);
        }

        public IEnumerable<RankEntry> GetFriendsRanks(int user_id)
        {
            // Wieso soll man Admin sein, um seinen Friend-Rank zu sehen?
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");
            
            return backend.Repository.StatisticsRepository.GetRankingWithFriends(user_id);
        }
    }
}