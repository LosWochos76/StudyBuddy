using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IGameBadgeService
    {
        IEnumerable<GameBadge> All();
        GameBadge GetById(int id);
        GameBadge Insert(GameBadge obj);
        GameBadge Update(GameBadge obj);
        void Delete(int id);
        
        IEnumerable<GameBadge> GetBadgesForChallenge(int challenge_id);
        public BadgeSuccessRate GetSuccessRate(int badge_id, int user_id);
    }
}