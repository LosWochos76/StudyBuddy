﻿using System;
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

            return new GameBadgeList()
            {
                Count = backend.Repository.GameBadges.GetCount(filter),
                Objects = backend.Repository.GameBadges.All(filter)
            };
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

        public GameBadgeList GetBadgesOfUser(int user_id)
        {
            if (backend.CurrentUser == null || (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id))
                throw new Exception("Unauthorized!");

            var objects = backend.Repository.GameBadges.GetBadgesOfUser(user_id);
            return new GameBadgeList()
            {
                Count = objects.Count(),
                Objects = objects
            };
        }
    }
}