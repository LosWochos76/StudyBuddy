using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IBusinessEventRepository
    {
        BusinessEvent GetById(int id);
        IEnumerable<BusinessEvent> All(BusinessEventFilter filter);
        BusinessEvent Insert(BusinessEvent obj);
        BusinessEvent Update(BusinessEvent obj);
        BusinessEvent Save(BusinessEvent obj);
        void Delete(int id);
    }
}