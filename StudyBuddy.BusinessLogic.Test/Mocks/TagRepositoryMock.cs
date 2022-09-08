using System;
using System.Collections.Generic;
using System.Linq;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class TagRepositoryMock : ITagRepository
    {
        private List<Tag> objects = new List<Tag>();
        private HashSet<Tuple<int, int>> tags_of_challenge = new HashSet<Tuple<int, int>>();
        private HashSet<Tuple<int, int>> tags_of_badge = new HashSet<Tuple<int, int>>();

        public TagRepositoryMock()
        {
        }

        public IEnumerable<Tag> All(TagFilter filter)
        {
            return objects;
        }

        public Tag ById(int id)
        {
            return objects.Where(obj => obj.ID == id).FirstOrDefault();
        }

        public Tag ByName(string name)
        {
            return objects.Where(obj => obj.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
        }

        public void Delete(int id)
        {
            objects.RemoveAll(obj => obj.ID == id);
        }

        public int GetCount(TagFilter filter)
        {
            return objects.Count;
        }

        public void Insert(Tag obj)
        {
            if (obj.ID == 0)
                obj.ID = GetCount(null) + 1;

            objects.Add(obj);
        }

        public void Save(Tag obj)
        {
            if (obj.ID == 0)
                Insert(obj);
            else
                Update(obj);
        }

        public void Update(Tag obj)
        {
            int pos = objects.FindIndex(o => o.ID == obj.ID);
            objects[pos] = obj;
        }

        public void AddTagForBadge(int tag_id, int badge_id)
        {
            tags_of_badge.Add(new Tuple<int, int>(tag_id, badge_id));
        }

        public IEnumerable<Tag> ForBadge(int badge_id)
        {
            foreach (var t in tags_of_badge)
                if (t.Item2 == badge_id)
                    yield return ById(t.Item1);
        }

        public void RemoveAllTagsFromBadge(int badge_id)
        {
            tags_of_badge.RemoveWhere(obj => obj.Item2 == badge_id);
        }

        public void AddTagForChallenge(int tag_id, int challenge_id)
        {
            tags_of_challenge.Add(new Tuple<int, int>(tag_id, challenge_id));
        }

        public IEnumerable<Tag> ForChallenge(int challenge_id)
        {
            foreach (var t in tags_of_challenge)
                if (t.Item2 == challenge_id)
                    yield return ById(t.Item1);
        }

        public void RemoveAllTagsFromChallenge(int challenge_id)
        {
            tags_of_challenge.RemoveWhere(obj => obj.Item2 == challenge_id);
        }

        public IEnumerable<ChallengeToBadgeDTO> GetChallengeToBadge()
        {
            foreach (var c in tags_of_challenge)
                foreach (var b in tags_of_badge)
                    if (c.Item1 == b.Item1)
                        yield return new ChallengeToBadgeDTO() { ChallengeID = c.Item2, BadgeID = b.Item2 };
        }
    }
}