﻿using StudyBuddy.Model.Model;
using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;
using System;

namespace StudyBuddy.Persistence
{
    internal class StatisticsRepository : IStatisticsRepository
    {
        public UserStatistics Statistics { get; set; }
        
        private readonly string connection_string;
        
        public StatisticsRepository(string connection_string)
        {
            this.connection_string = connection_string;
        }

        public UserStatistics GetUserStatistics(int user_id)
        {
            AddChallengeStatistics(user_id); 
            
            AddChallengeHistory(user_id);
            Statistics.CalculateStatisticTrends();
            return Statistics;
        }

        private void AddChallengeStatistics(int user_id)
        {
            var qh = new QueryHelper<ChallengeStatisticsDto>(connection_string, ChallengeStatisticsReader);

            qh.AddParameter(":user_id", user_id);

            var sql = "select category, sum(points), count(category) from challenge_acceptance " +
                "inner join challenges on challenge_id = id where user_id = :user_id group by category";

            var challengeStatisticsDtoList = qh.ExecuteQueryToObjectList(sql);
            Statistics = TransformDto(challengeStatisticsDtoList);
            
        }

        /*
        public UserStatistics GetUserStatistics(int user_id)
        {
            var qh = new QueryHelper<ChallengeStatisticsDto>(connection_string, ChallengeStatisticsReader);

            qh.AddParameter(":user_id", user_id);

            var sql = "select category, sum(points) as points, count(category) as categoryCount from challenge_acceptance " +
                "inner join challenges on challenge_id = id where user_id = :user_id group by category";

            var challengeStatisticsDtoList = qh.ExecuteQueryToObjectList(sql);
            Statistics = TransformDto(challengeStatisticsDtoList);
            Statistics.UserId = user_id;
            Statistics.FriendsRank = GetRankingWithFriends(user_id);
            AddChallengeHistory(user_id);

            Statistics.AddStatisticTrends();
            return Statistics;
        }
        */

        public void AddChallengeHistory(int user_id)
        {
            var qh = new QueryHelper<UserStatistics>(connection_string, ChallengeHistoryReader);

            qh.AddParameter(":user_id", user_id);

            var sql = "Select " +
                    "count(case when created > NOW() - interval '2 week' and created < NOW() - interval '1 week' then 1 end) as count_last_week, " +
                    "count(case when created > NOW() - interval '1 week' and created < NOW() then 1 end) as count_current_week " +
                    "from challenge_acceptance where user_id = :user_id";

            var result = qh.ExecuteQueryToSingleObject(sql);
        }


        //public void AddChallengeHistory(int user_id)
        //{
        //    var qh = new QueryHelper<UserStatistics>(connection_string, ChallengeHistoryReader);

        //    qh.AddParameter(":user_id", user_id);

        //    var sql = "Select " +
        //            "count(case when created > NOW() - interval '2 week' and created < NOW() - interval '1 week' then 1 end) as count_last_week, " +
        //            "count(case when created > NOW() - interval '1 week' and created < NOW() then 1 end) as count_current_week, " +
        //            "count(case when created > NOW() - interval '2 month' and created < NOW() - interval '1 month' then 1 end) as count_last_month, " +
        //            "count(case when created > NOW() - interval '1 month' and created < NOW() then 1 end) as count_this_month " +
        //            "from challenge_acceptance where user_id = :user_id";

        //    var result = qh.ExecuteQueryToSingleObject(sql);
        //}

        public UserStatistics ChallengeHistoryReader(NpgsqlDataReader reader)
        { 
            Statistics.LastWeekChallengeCount = reader.GetInt32(0);
            Statistics.ThisWeekChallengeCount = reader.GetInt32(1);
            //Statistics.LastMonthChallengeCount = reader.GetInt32(2);
            //Statistics.ThisMonthChallengeCount = reader.GetInt32(3);

            return Statistics;
        }

        public IEnumerable<RankEntry> GetRankingWithFriends(int user_id)
        {
            var qh = new QueryHelper<RankEntry>(connection_string, RankReader);

            qh.AddParameter(":user_id", user_id);

            var sql = "select total_rank, nickname, total_points from(Select user_id, us.nickname, sum(points) as total_points, RANK() over " +
                    "(order by sum(points) desc) as total_rank from challenge_acceptance inner join challenges as ch on challenge_id=id " +
                    "inner join users as us on user_id=us.id where user_id in (select user_b from friends where user_a = :user_id) or user_id = :user_id " +
                    "group by user_id, us.nickname)  sub";

            var ranking = qh.ExecuteQueryToObjectList(sql);
            
            return ranking;
        }

        public IEnumerable<ChallengeHistoryEntry> GetChallengeHistoryWeekly(int user_id)
        {
            var qh = new QueryHelper<ChallengeHistoryEntry>(connection_string, ChallengeHistoryDetailReader);

            qh.AddParameter(":user_id", user_id);

            var sql = "SELECT DATE_trunc('week',ca.created) as week, points, category" +
                    "FROM public.challenge_acceptance as ca inner join challenges on id = challenge_id where user_id = :user_id";

            var challengeHistory = qh.ExecuteQueryToObjectList(sql);
            
            return challengeHistory;
        }

        private UserStatistics TransformDto(IEnumerable<ChallengeStatisticsDto> csDtoList)
        {
            var userStatistic = new UserStatistics();
            foreach(var dto in csDtoList)
            {
                switch (dto.ChallengeCategoryId)
                {
                    case (int)ChallengeCategory.Learning:
                        userStatistic.TotalLearningChallengesPoints = dto.Points;
                        userStatistic.TotalLearningChallengesCount = dto.ChallengeCount;
                        continue;
                    case (int)ChallengeCategory.Networking:
                        userStatistic.TotalNetworkChallengesPoints = dto.Points;
                        userStatistic.TotalNetworkChallengesCount = dto.ChallengeCount;
                        continue;
                    case (int)ChallengeCategory.Organizing:
                        userStatistic.TotalOrganizingChallengesPoints = dto.Points;
                        userStatistic.TotalOrganizingChallengesCount = dto.ChallengeCount;
                        continue;
                    default:
                        break;
                }
            }
            return userStatistic;
        }

        public ChallengeStatisticsDto ChallengeStatisticsReader(NpgsqlDataReader reader)
        {
            var challengeStatisticsDto = new ChallengeStatisticsDto();

            challengeStatisticsDto.ChallengeCategoryId = reader.GetInt32(0);
            challengeStatisticsDto.Points = reader.GetInt32(1);
            challengeStatisticsDto.ChallengeCount = reader.GetInt32(2);
            
            return challengeStatisticsDto;
        }

        public RankEntry RankReader(NpgsqlDataReader reader)
        {
            var rankEntry = new RankEntry();
            rankEntry.Rank = reader.GetInt32(0);
            rankEntry.Nickname = reader.GetString(1);
            rankEntry.Total_points = reader.GetInt32(2);
            return rankEntry;
        }

        public ChallengeHistoryEntry ChallengeHistoryDetailReader(NpgsqlDataReader reader)
        {
            var history = new ChallengeHistoryEntry();
            history.Week = reader.GetDateTime(0);
            history.Points = reader.GetInt32(1);
            history.ChallengeCategory = reader.GetInt32(2);
            return history;
        }
                
        public class ChallengeStatisticsDto
        {
            public int ChallengeCategoryId  { get; set; }

            public int Points { get; set; }

            public int ChallengeCount { get; set; }
        }

        public class ChallengeHistoryEntry
        {
            public DateTime Week { get; set; }

            public int Points { get; set; }

            public int ChallengeCategory { get; set; }

        }
    }
}
