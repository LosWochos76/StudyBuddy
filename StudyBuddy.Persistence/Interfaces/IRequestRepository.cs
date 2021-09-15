using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IRequestRepository
    {
        Request ById(int id);
        IEnumerable<Request> All(int from = 0, int max = 1000);
        IEnumerable<Request> OfSender(int owner_id);
        IEnumerable<Request> ForRecipient(int recipient_id);
        void Insert(Request obj);
        void Delete(int id);
    }
}
