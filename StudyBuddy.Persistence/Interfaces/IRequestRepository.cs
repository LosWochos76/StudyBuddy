using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IRequestRepository
    {
        Request ById(int id);
        IEnumerable<Request> All(int from = 0, int max = 1000);
        Request FindSimilar(Request obj);
        IEnumerable<Request> OfSender(int sender_id);
        IEnumerable<Request> ForRecipient(int recipient_id);
        void Insert(Request obj);
        void Delete(int id);
    }
}