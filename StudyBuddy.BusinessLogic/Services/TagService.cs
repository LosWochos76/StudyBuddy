using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class TagService : ITagService
    {
        private readonly IBackend backend;

        public TagService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<Tag> GetAll()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.All();
        }

        public int GetCount()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.Count();
        }

        public Tag GetById(int id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.ById(id);
        }

        public Tag Update(Tag obj)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.Update(obj);
            return obj;
        }

        public Tag Insert(Tag obj)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.Delete(id);
        }

        private IEnumerable<string> SplitTags(string tags)
        {
            return tags
                .Replace("#", "")
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private Tag CreateOrFindSingle(string tag_name)
        {
            tag_name = tag_name.ToLower();
            var obj = backend.Repository.Tags.ByName(tag_name);

            if (obj == null)
            {
                obj = new Tag { Name = tag_name };
                Insert(obj);
            }

            return obj;
        }

        public IEnumerable<Tag> CreateOrFindMultiple(string tags)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new Exception("Unauthorized!");

            var tag_list = SplitTags(tags);
            var object_list = new List<Tag>();

            foreach (var tag in tag_list)
                object_list.Add(CreateOrFindSingle(tag));

            return object_list;
        }

        public IEnumerable<Tag> OfChallenge(int challenge_id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.OfChallenge(challenge_id);
        }

        public IEnumerable<Tag> SetForChallenge(TagsForChallengeParameter parameter)
        {
            var challenge = backend.Repository.Challenges.ById(parameter.ChallengeId);
            if (challenge == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && challenge.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.RemoveAllTagsFromChallenge(parameter.ChallengeId);

            var tags = CreateOrFindMultiple(parameter.Tags);
            foreach (var tag in tags)
                backend.Repository.Tags.AddTagForChallenge(tag.ID, parameter.ChallengeId);

            return tags;
        }

        public IEnumerable<Tag> OfBadge(int badge_id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            return backend.Repository.Tags.OfBadge(badge_id);
        }

        public IEnumerable<Tag> SetForBadge(TagsForBadgeParameter parameter)
        {
            var badge = backend.Repository.GameBadges.ById(parameter.BadgeId);
            if (badge == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && badge.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized!");

            backend.Repository.Tags.RemoveAllTagsFromBadge(parameter.BadgeId);

            var tags = CreateOrFindMultiple(parameter.Tags);
            foreach (var tag in tags)
                backend.Repository.Tags.AddTagForBadge(tag.ID, parameter.BadgeId);

            return tags;
        }
    }
}