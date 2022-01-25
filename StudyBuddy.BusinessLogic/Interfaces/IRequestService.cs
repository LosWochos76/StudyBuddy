using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic
{
    public interface IRequestService
    {
        RequestList All(RequestFilter filter);
        void Delete(int id);
        void Deny(int id);
        Request GetById(int id);
        Request Insert(Request obj);
        void Accept(int id);
    }
}