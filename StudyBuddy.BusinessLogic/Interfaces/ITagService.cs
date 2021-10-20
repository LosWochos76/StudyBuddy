using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface ITagService
    {
        IEnumerable<Tag> CreateOrFindMultiple(string tags);
        void Delete(int id);
        IEnumerable<Tag> GetAll();
        Tag GetById(int id);
        int GetCount();
        Tag Insert(Tag obj);
        IEnumerable<Tag> OfChallenge(int challenge_id);
        IEnumerable<Tag> SetForChallenge(TagsForChallengeParameter parameter);
        Tag Update(Tag obj);
    }
}