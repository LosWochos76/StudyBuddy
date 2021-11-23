namespace StudyBuddy.Persistence.Repositories
{
    using StudyBuddy.Model;
    using StudyBuddy.Model.Model;
    using StudyBuddy.Persistence.Interfaces;
    using System;
    using System.Collections.Generic;

    internal class StatisticsRepository : IStatisticsRepository
    {
        private readonly string connection_string;

        public StatisticsRepository(string connection_string)
        {
            this.connection_string = connection_string;
        }

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

        public int RankingPositionForUser(int user_id)
        {
            throw new NotImplementedException();
        }

        public int AcceptedChallengesCountForUser(int user_id)
        {
            var qh = new QueryHelper<Statistics>(connection_string);

            qh.AddParameter(":user_id", user_id);
            return qh.ExecuteQueryToSingleInt("Select count(*) from challenge_acceptance where user_id = :user_id");
        }

        //public IEnumerable<KeyValuePair<string, int>> ()
        //{
        //    var sql = "select user_id, us.nickname, sum(points) as total_points, RANK() over (order by sum(points) desc) as total_rank " +
        //        "from challenge_acceptance inner join challenges as ch  on challenge_id=id inner join users as us on user_id=us.id group by user_id, us.nickname";
        //    throw new NotImplementedException();
        //}
    }
}
