using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IBusinessEventService
    {
        void TriggerEvent(object sender, BusinessEventArgs args);

        // CRUD:
        IEnumerable<BusinessEvent> All(BusinessEventFilter filter);
        void Delete(int id);
        BusinessEvent GetById(int id);
        BusinessEvent Insert(BusinessEvent obj);
        BusinessEvent Update(BusinessEvent obj);
    }
}