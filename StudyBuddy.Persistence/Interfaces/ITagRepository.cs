using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface ITagRepository
    {
        Tag ById(int id);
        Tag ByName(string name);
        int Count();
        IEnumerable<Tag> All(int from = 0, int max = 1000);
        void Insert(Tag obj);
        void Update(Tag obj);
        void Save(Tag obj);
        void Delete(int id);

        public IEnumerable<Tag> OfChallenge(int challenge_id);
        void AddTagForChallenge(int tag_id, int user_id);
        void RemoveAllTagsFromChallenge(int challenge_id);

        IEnumerable<Tag> OfBadge(int badge_id);
        void RemoveAllTagsFromBadge(int badge_id);
        void AddTagForBadge(int tag_id, int badge_id);
    }
}