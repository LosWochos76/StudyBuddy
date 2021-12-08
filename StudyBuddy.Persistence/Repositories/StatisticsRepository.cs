using StudyBuddy.Model.Model;
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

            userStatistic.UserId = user_id;
            userStatistic.FriendsRank = GetRankingWithFriends(user_id);

            return userStatistic;
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

        public class ChallengeStatisticsDto
        {
            public int ChallengeCategoryId  { get; set; }

            public int Points { get; set; }

            public int ChallengeCount { get; set; }
        }
    }
}
