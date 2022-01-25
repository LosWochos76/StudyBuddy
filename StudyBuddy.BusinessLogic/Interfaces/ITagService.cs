using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic
{
    public interface ITagService
    {
        TagList GetAll(TagFilter filter);
        Tag GetById(int id);
        Tag Insert(Tag obj);
        Tag Update(Tag obj);
        void Delete(int id);

        TagList CreateOrFindMultiple(string tags);
    }
}