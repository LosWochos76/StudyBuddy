using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class RequestService : IRequestService
    {
        private readonly IBackend backend;

        public RequestService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<Request> All()
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            return backend.Repository.Requests.All();
        }

        public IEnumerable<Request> ForRecipient(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            return backend.Repository.Requests.ForRecipient(user_id);
        }

        public IEnumerable<Request> OfSender(int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != user_id)
                throw new Exception("Unauthorized!");

            return backend.Repository.Requests.OfSender(user_id);
        }

        public Request GetById(int id)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            var request = backend.Repository.Requests.ById(id);
            if (!backend.CurrentUser.IsAdmin && request.SenderID != backend.CurrentUser.ID && request.RecipientID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized!");

            return request;
        }

        public Request Insert(Request obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != obj.SenderID)
                throw new Exception("Unauthorized!");

            backend.Repository.Requests.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            var obj = backend.RequestService.GetById(id);
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null ||
                !backend.CurrentUser.IsAdmin && (backend.CurrentUser.ID != obj.SenderID  && backend.CurrentUser.ID != obj.RecipientID))
                throw new Exception("Unauthorized!");

            backend.Repository.Requests.Delete(id);
        }

        public void Accept(int id)
        {
            var obj = backend.Repository.Requests.ById(id);

            if (obj == null)
                throw new Exception("Object not found!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != obj.RecipientID)
                throw new Exception("Unauthorized!");

            var user = backend.Repository.Users.ById(obj.SenderID);

            if (obj.Type == RequestType.Friendship)
            {
                backend.Repository.Users.AddFriend(obj.SenderID, obj.RecipientID);
                var friend = backend.Repository.Users.ById(obj.RecipientID);
                backend.BusinessEvent.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.FriendAdded, friend) { CurrentUser = user });
            }
            else if (obj.Type == RequestType.ChallengeAcceptance)
            {
                backend.Repository.Challenges.AddAcceptance(obj.ChallengeID.Value, obj.SenderID);
                var challenge = backend.Repository.Challenges.ById(obj.ChallengeID.Value);
                backend.BusinessEvent.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.ChallengeAccepted, challenge) { CurrentUser = user });
            }

            backend.Repository.Requests.Delete(id);

            // ToDo: Erzeugen einer Neuigkeit!

        }

        public void Deny(int id)
        {
            var obj = backend.Repository.Requests.ById(id);
            if (obj == null)
                throw new Exception("Object not found!");

            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != obj.RecipientID)
                throw new Exception("Unauthorized!");

            backend.Repository.Requests.Delete(id);
        }
    }
}