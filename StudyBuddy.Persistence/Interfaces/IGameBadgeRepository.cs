using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IGameBadgeRepository
    {
        GameBadge ById(int id);
        IEnumerable<GameBadge> All(GameBadgeFilter filter);
        int GetCount(GameBadgeFilter filter);
        void Save(GameBadge obj);
        void Insert(GameBadge obj);
        void Update(GameBadge obj);
        void Delete(int id);

        IEnumerable<GameBadge> GetBadgesForChallenge(int challenge_id);
        BadgeSuccessRate GetSuccessRate(int badge_id, int user_id);

        void AddBadgeToUser(int user_id, int badge_id);
        void RemoveBadgeFromUser(int user_id, int badge_id);
        void RemoveAllBadgesFromUser(int user_id);
        IEnumerable<GameBadge> GetReceivedBadgesOfUser(int user_id, GameBadgeFilter filter);
    }
}