using StudyBuddy.Model;
using StudyBuddy.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Persistence.Interfaces
{
    public interface IStatisticsRepository
    {        
        IEnumerable<Challenge> AcceptedChallengesForUser(int user_id);

        Statistics ChallengeStatisticForUser(int user_id);

        IEnumerable<Badge> BadgesEarnedByUser(int user_id);

        int RankingPositionForUser(int user_id);

        int AcceptedChallengesCountForUser(int user_id);
        //IEnumerable<KeyValuePair<string, int>> UserRankingByChallengePoints();
    }
}
