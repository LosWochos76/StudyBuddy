using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IImageRepository
    {
        PersistentImage ById(int id);
        IEnumerable<PersistentImage> All(ImageFilter filter);
        PersistentImage Save(PersistentImage obj);
        PersistentImage Insert(PersistentImage obj);
        PersistentImage Update(PersistentImage obj);
        void Delete(int id);

        PersistentImage OfUser(int user_id);
    }
}