using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IBusinessEventService
    {
        // CRUD:
        BusinessEventList All(BusinessEventFilter filter);
        void Delete(int id);
        BusinessEvent GetById(int id);
        BusinessEvent Insert(BusinessEvent obj);
        BusinessEvent Update(BusinessEvent obj);

        // Misc
        void TriggerEvent(object sender, BusinessEventArgs args);
        void Execute(int id);
        public IEnumerable<string> Compile(BusinessEvent obj);
    }
}