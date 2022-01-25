using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    public interface ITagRepository
    {
        IEnumerable<Tag> All(TagFilter filter);
        int GetCount(TagFilter filter);
        Tag ById(int id);
        Tag ByName(string name);
        void Insert(Tag obj);
        void Update(Tag obj);
        void Save(Tag obj);
        void Delete(int id);

        void AddTagForChallenge(int tag_id, int user_id);
        void RemoveAllTagsFromChallenge(int challenge_id);

        void RemoveAllTagsFromBadge(int badge_id);
        void AddTagForBadge(int tag_id, int badge_id);
    }
}