using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IGameBadgeRepository
    {
        GameBadge ById(int id);
        IEnumerable<GameBadge> All(int from = 0, int max = 1000);
        void Save(GameBadge obj);
        void Delete(int id);
        void AddChallenge(int badge_id, int challenge_id);
        void RemoveChallenge(int badge_id, int challenge_id);
    }
}
