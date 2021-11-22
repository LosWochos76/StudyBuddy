namespace StudyBuddy.Persistence.Repositories
{
    using StudyBuddy.Model;
    using StudyBuddy.Model.Model;
    using StudyBuddy.Persistence.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class StatisticsRepository : IStatisticsRepository
    {
        public IEnumerable<Challenge> AcceptedChallengesForUser(int user_id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Badge> BadgesEarnedByUser(int user_id)
        {
            throw new NotImplementedException();
        }

        public Statistics ChallengeStatisticForUser(int user_id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, int>> UserRankingByChallengePoints()
        {
            var sql = "select user_id, us.nickname, sum(points) as total_points, RANK() over (order by sum(points) desc) as total_rank " +
                "from challenge_acceptance inner join challenges as ch  on challenge_id=id inner join users as us on user_id=us.id group by user_id, us.nickname";
            throw new NotImplementedException();
        }
    }
}
