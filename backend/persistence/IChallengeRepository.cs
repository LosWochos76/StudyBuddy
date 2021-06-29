using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IChallengeRepository
    {
        Challenge ById(int id);
        IEnumerable<Challenge> All(int from = 0, int max = 1000);
        void Save(Challenge obj);
        void Delete(int id);
    }
}
