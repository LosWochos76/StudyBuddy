using System;
using System.Collections.Generic;
using System.Linq;
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

        public GameBadgeList All(GameBadgeFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if (filter == null)
                filter = new GameBadgeFilter();

            var count = backend.Repository.GameBadges.GetCount(filter);
            var objects = backend.Repository.GameBadges.All(filter);

            if (filter.WithOwner)
            {
                foreach (var obj in objects)
                {
                    obj.Owner = backend.Repository.Users.ById(obj.OwnerID);
                }
            }

            return new GameBadgeList() { Count = count, Objects = objects };
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
            backend.Repository.Tags.RemoveAllTagsFromBadge(obj.ID);
            foreach (var tag in backend.TagService.CreateOrFindMultiple(obj.Tags).Objects)
                backend.Repository.Tags.AddTagForBadge(tag.ID, obj.ID);

            return obj;
        }

        public GameBadge Insert(GameBadge obj)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new Exception("Unauthorized");

            if (obj == null)
                throw new Exception("Object invalid!");

            backend.Repository.GameBadges.Insert(obj);
            foreach (var tag in backend.TagService.CreateOrFindMultiple(obj.Tags).Objects)
                backend.Repository.Tags.AddTagForBadge(tag.ID, obj.ID);

            return obj;
        }

        public void Delete(int id)
        {
            var obj = backend.Repository.GameBadges.ById(id);
            if (obj == null)
                throw new Exception("Object not found!");

            if (backend.CurrentUser == null || (!backend.CurrentUser.IsAdmin && obj.OwnerID != backend.CurrentUser.ID))
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.RemoveAllTagsFromBadge(obj.ID);
            backend.Repository.GameBadges.Delete(id);
        }

        public GameBadgeList GetBadgesForChallenge(int challenge_id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            var objects = backend.Repository.GameBadges.GetBadgesForChallenge(challenge_id);
            return new GameBadgeList()
            {
                Count = objects.Count(),
                Objects = objects
            };
        }

        public BadgeSuccessRate GetSuccessRate(int badge_id, int user_id)
        {
            if (backend.CurrentUser == null || (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id))
                throw new Exception("Unauthorized!");

            return backend.Repository.GameBadges.GetSuccessRate(badge_id, user_id);
        }

        public void AddBadgeToUser(int user_id, int badge_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            var obj = backend.Repository.GameBadges.ById(badge_id);
            if (obj == null)
                throw new Exception("Object not found!");

            backend.Repository.GameBadges.AddBadgeToUser(user_id, badge_id);
        }

        public void RemoveBadgeFromUser(int user_id, int badge_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.GameBadges.RemoveBadgeFromUser(user_id, badge_id);
        }

        public GameBadgeList GetReceivedBadgesOfUser(int user_id, GameBadgeFilter filter)
        {
            if (backend.CurrentUser == null || (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id))
                throw new Exception("Unauthorized!");

            var objects = backend.Repository.GameBadges.GetReceivedBadgesOfUser(user_id, filter);
            return new GameBadgeList()
            {
                Count = objects.Count(),
                Objects = objects
            };
        }

        public bool CheckIfUserEarnedGameBadgeAfterChallengeCompleted(User user, Challenge challenge)
        {
            var filter = new GameBadgeFilter() { Count = int.MaxValue };
            var badges_of_user = GetReceivedBadgesOfUser(user.ID, filter);
            var badges = backend.Repository.GameBadges.GetBadgesForChallenge(challenge.ID);
            var result = false;

            foreach (var badge in badges)
            {
                if (!IsInList(badges_of_user.Objects, badge))
                {
                    var success_rate = backend.GameBadgeService.GetSuccessRate(badge.ID, user.ID);
                    if (success_rate.Success >= badge.RequiredCoverage)
                    {
                        backend.Repository.GameBadges.AddBadgeToUser(user.ID, badge.ID);
                        backend.NotificationService.UserReceivedBadge(user, badge);
                        result = true;

                        // ToDo: Raise Event!
                    }
                }
            }

            return result;
        }

        private bool IsInList(IEnumerable<GameBadge> list, GameBadge badge)
        {
            foreach (var b in list)
                if (b.ID.Equals(badge.ID))
                    return true;

            return false;
        }
    }
}