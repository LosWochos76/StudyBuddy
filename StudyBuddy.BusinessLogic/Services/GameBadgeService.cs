using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class GameBadgeService
    {
        private readonly IRepository repository;

        public GameBadgeService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<GameBadge> All(User current_user)
        {
            if (current_user == null || !current_user.IsAdmin)
                return repository.GameBadges.OfOwner(current_user.ID);
            return repository.GameBadges.All();
        }

        public GameBadge GetById(int id)
        {
            return repository.GameBadges.ById(id);
        }

        public GameBadge Update(User current_user, GameBadge obj)
        {
            if (obj == null)
                throw new Exception("Object invalid!");

            if (!current_user.IsAdmin && obj.OwnerID != current_user.ID)
                throw new Exception("Unauthorized");

            repository.GameBadges.Update(obj);
            return obj;
        }

        public GameBadge Insert(GameBadge obj)
        {
            if (obj == null)
                throw new Exception("Object invalid!");

            repository.GameBadges.Insert(obj);
            return obj;
        }

        public void Delete(User current_user, int id)
        {
            var obj = repository.GameBadges.ById(id);
            if (obj == null)
                throw new Exception("Object not found!");

            if (!current_user.IsAdmin && obj.OwnerID != current_user.ID)
                throw new Exception("Unauthorized!");

            repository.GameBadges.Delete(id);
        }

        public void SetChallenges(GameBadgeChallenge[] challenges)
        {
            if (challenges.Length == 0)
                throw new Exception("No challenges delivered!");

            repository.GameBadges.DeleteChallenges(challenges[0].GameBadgeId);
            repository.GameBadges.AddChallenges(challenges);
        }
    }
}