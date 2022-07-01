using System.Collections.Generic;
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
            var obj = new UserStatistics() { UserId = user_id };
            FillWithPoints(obj);
            FillWithRankingWithFriends(obj);
            FillWithChallengeHistory(obj);
            return obj;
        }

        private void FillWithPoints(UserStatistics obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", obj.UserId);

            var sql = "select category, sum(points) as points, count(category) as challenge_count from challenge_acceptance " +
                "inner join challenges on challenge_id = id where user_id = :user_id group by category";

            var set = qh.ExecuteQuery(sql);
            for (int row = 0; row < set.RowCount; row++)
            {
                var category = set.GetInt(row, "category");
                var points = set.GetInt(row, "category");
                var count = set.GetInt(row, "challenge_count");

                switch (category)
                {
                    case (int)ChallengeCategory.Learning:
                        obj.TotalLearningChallengesPoints = points;
                        obj.TotalLearningChallengesCount = count;
                        continue;
                    case (int)ChallengeCategory.Networking:
                        obj.TotalNetworkChallengesPoints = points;
                        obj.TotalNetworkChallengesCount = count;
                        continue;
                    case (int)ChallengeCategory.Organizing:
                        obj.TotalOrganizingChallengesPoints = points;
                        obj.TotalOrganizingChallengesCount = count;
                        continue;
                }
            }
        }

        private void FillWithRankingWithFriends(UserStatistics obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", obj.UserId);

            var sql = "select total_rank, nickname, total_points from(Select user_id, us.nickname, sum(points) as total_points, RANK() over " +
                    "(order by sum(points) desc) as total_rank from challenge_acceptance inner join challenges as ch on challenge_id=id " +
                    "inner join users as us on user_id=us.id where user_id in (select user_b from friends where user_a = :user_id) or user_id = :user_id " +
                    "group by user_id, us.nickname)  sub";

            var ranking = new List<RankEntry>();
            var set = qh.ExecuteQuery(sql);
            for (int row = 0; row < set.RowCount; row++)
            {
                var rank = new RankEntry();
                rank.Rank = set.GetInt(row, "total_rank");
                rank.Nickname = set.GetString(row, "nickname");
                rank.TotalPoints = set.GetInt(row, "total_points");
                ranking.Add(rank);
            }

            obj.FriendsRank = ranking;
        }

        private void FillWithChallengeHistory(UserStatistics obj)
        {
            var qh = new QueryHelper(connection_string);
            qh.AddParameter(":user_id", obj.UserId);

            var sql = "Select " +
                    "count(case when created > NOW() - interval '2 week' and created < NOW() - interval '1 week' then 1 end) as count_last_week, " +
                    "count(case when created > NOW() - interval '1 week' and created < NOW() then 1 end) as count_current_week, " +
                    "count(case when created > NOW() - interval '2 month' and created < NOW() - interval '1 month' then 1 end) as count_last_month, " +
                    "count(case when created > NOW() - interval '1 month' and created < NOW() then 1 end) as count_this_month " +
                    "from challenge_acceptance where user_id = :user_id";

            var set = qh.ExecuteQuery(sql);
            obj.LastWeekChallengeCount = set.GetInt(0, "count_last_week");
            obj.ThisWeekChallengeCount = set.GetInt(0, "count_current_week");
            obj.LastMonthChallengeCount = set.GetInt(0, "count_last_month");
            obj.ThisMonthChallengeCount = set.GetInt(0, "count_this_month");
        }
    }
}