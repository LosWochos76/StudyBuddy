using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class GameBadgeService : IGameBadgeService
    {
        private readonly IBackend backend;

        public GameBadgeService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<GameBadge> All()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                return backend.Repository.GameBadges.OfOwner(backend.CurrentUser.ID);

            return backend.Repository.GameBadges.All();
        }

        public GameBadge GetById(int id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized");

            return backend.Repository.GameBadges.ById(id);
        }

        public GameBadge Update(GameBadge obj)
        {
            if (obj == null)
                throw new Exception("Object invalid!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && obj.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized");

            backend.Repository.GameBadges.Update(obj);
            return obj;
        }

        public GameBadge Insert(GameBadge obj)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new Exception("Unauthorized");

            if (obj == null)
                throw new Exception("Object invalid!");

            backend.Repository.GameBadges.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            var obj = backend.Repository.GameBadges.ById(id);
            if (obj == null)
                throw new Exception("Object not found!");

            if (!backend.CurrentUser.IsAdmin && obj.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized!");

            backend.Repository.GameBadges.Delete(id);
        }

        // ToDo: Der Parameter hier ist Mist! Sollte nur eine GameBadgeId und viele ChallengeIds sein! Dann muss aber auch das Angular-Backend abgepasst werden, weil sich die Signatur ändert!
        public void SetChallenges(GameBadgeChallenge[] challenges)
        {
            if (challenges == null || challenges.Length == 0)
                throw new Exception("No challenges delivered!");

            var obj = backend.Repository.GameBadges.ById(challenges[0].GameBadgeId);
            if (backend.CurrentUser == null || backend.CurrentUser.IsAdmin && obj.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized!");

            backend.Repository.GameBadges.DeleteChallenges(challenges[0].GameBadgeId);
            backend.Repository.GameBadges.AddChallenges(challenges);
        }
    }
}