using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IGameBadgeService
    {
        GameBadgeList All(GameBadgeFilter filter);
        GameBadge GetById(int id);
        GameBadge Insert(GameBadge obj);
        GameBadge Update(GameBadge obj);
        void Delete(int id);

        GameBadgeList GetBadgesForChallenge(int challenge_id);
        public BadgeSuccessRate GetSuccessRate(int badge_id, int user_id);

        void AddBadgeToUser(int user_id, int badge_id);
        void RemoveBadgeFromUser(int user_id, int badge_id);
        GameBadgeList GetBadgesOfUser(int user_id);
    }
}