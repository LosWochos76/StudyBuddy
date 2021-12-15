using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IImageService
    {
        PersistentImage GetById(int id);
        IEnumerable<PersistentImage> All(ImageFilter filter);
        PersistentImage Insert(PersistentImage obj);
        PersistentImage Update(PersistentImage obj);
        void Delete(int id);

        PersistentImage GetOfUser(int user_id);
        PersistentImage SaveOfUser(PersistentImage obj);
    }
}