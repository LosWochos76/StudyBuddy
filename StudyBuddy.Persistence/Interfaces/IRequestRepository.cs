using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.Persistence
{
    public interface IRequestRepository
    {
        Request ById(int id);
        IEnumerable<Request> All(RequestFilter filter);
        void Insert(Request obj);
        void Delete(int id);

        Request FindSimilar(Request obj);
        Request FindFriendshipRequest(int sender_id, int recipient_id);
    }
}