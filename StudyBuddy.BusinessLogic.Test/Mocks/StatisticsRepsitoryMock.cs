using StudyBuddy.Model;
using StudyBuddy.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class StatisticsRepsitoryMock : IStatisticsRepository
    {
        private List<UserStatistics> _stats = new();

        public IEnumerable<GameObjectStatistics> GetBadgeCompletionStatistics(bool orderAscending)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GameBadge> GetBadgeStatistic(bool orderAscending)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GameObjectStatistics> GetChallengeCompletionStatistics(bool orderAscending)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Challenge> GetChallengeStatistic(bool orderAscending)
        {
            throw new NotImplementedException();
        }

        public UserStatistics GetUserStatistics(int user_id)
        {
            return _stats.Where(obj => obj.UserId.Equals(user_id)).FirstOrDefault();
        }

        public IEnumerable<User> GetUsersWithDateCreated(bool orderAscending)
        {
            throw new NotImplementedException();
        }
    }
}
