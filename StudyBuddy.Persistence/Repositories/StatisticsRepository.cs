using StudyBuddy.Model.Model;
using System;
using System.Collections.Generic;
using Npgsql;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    internal class StatisticsRepository : IStatisticsRepository
    {
        private readonly string connection_string;
        public StatisticsRepository(string connection_string)
        {
            this.connection_string = connection_string;
        }

        public UserStatistics GetUserStatistics(int user_id)
        {
            var qh = new QueryHelper<ChallengeStatisticsDto>(connection_string, ChallengeStatisticsReader);

            qh.AddParameter(":user_id", user_id);

            var sql = "select category, sum(points) as points, count(category) as categoryCount from challenge_acceptance " +
                "inner join challenges on challenge_id = id where user_id = :user_id group by category";

            var challengeStatisticsDtoList = qh.ExecuteQueryToObjectList(sql);
            var userStatistic = TransformDto(challengeStatisticsDtoList);

            return userStatistic;
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

        public class ChallengeStatisticsDto
        {
            public int ChallengeCategoryId  { get; set; }

            public int Points { get; set; }

            public int ChallengeCount { get; set; }
        }
    }
}
