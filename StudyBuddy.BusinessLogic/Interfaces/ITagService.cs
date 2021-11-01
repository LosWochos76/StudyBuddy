using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
        Tag GetById(int id);
        int GetCount();
        Tag Insert(Tag obj);
        Tag Update(Tag obj);
        void Delete(int id);

        IEnumerable<Tag> CreateOrFindMultiple(string tags);
    }
}