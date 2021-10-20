using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IRequestService
    {
        void Accept(int id);
        IEnumerable<Request> All();
        void Delete(int id);
        void Deny(int id);
        IEnumerable<Request> ForRecipient(int user_id);
        Request GetById(int id);
        Request Insert(Request obj);
        IEnumerable<Request> OfSender(int user_id);
    }
}