using System;
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

        // event
        event RequestEventHandler RequestCreated;
        event RequestEventHandler RequestAccepted;
        event RequestEventHandler RequestDenied;
    }

    public delegate void RequestEventHandler(object sender, RequestEventArgs e);

    public class RequestEventArgs : EventArgs
    {
        public Request Request { get; set; }

        public RequestEventArgs(Request r)
        {
            Request = r;
        }
    }
}