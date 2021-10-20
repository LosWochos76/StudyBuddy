using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IGameBadgeService
    {
        IEnumerable<GameBadge> All();
        void Delete(int id);
        GameBadge GetById(int id);
        GameBadge Insert(GameBadge obj);
        void SetChallenges(GameBadgeChallenge[] challenges);
        GameBadge Update(GameBadge obj);
    }
}