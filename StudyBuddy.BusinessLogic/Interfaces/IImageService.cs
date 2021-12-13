using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IImageService
    {
        PersistentImage GetById(int id);
        PersistentImage OfUser(int user_id);
        IEnumerable<PersistentImage> All(ImageFilter filter);
        PersistentImage Insert(PersistentImage obj);
        PersistentImage Update(PersistentImage obj);
        void Delete(int id);   
    }
}