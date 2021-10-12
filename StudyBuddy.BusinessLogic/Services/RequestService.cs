using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class RequestService
    {
        private User current_user;
        private readonly IRepository repository;

        public RequestService(IRepository repository, User current_user)
        {
            this.repository = repository;
            this.current_user = current_user;
        }

        public IEnumerable<Request> All()
        {
            return repository.Requests.All();
        }

        public IEnumerable<Request> ForRecipient(int user_id)
        {
            if (!current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            return repository.Requests.ForRecipient(user_id);
        }

        public IEnumerable<Request> OfSender(int user_id)
        {
            if (!current_user.IsAdmin && current_user.ID != user_id)
                throw new Exception("Unauthorized!");

            return repository.Requests.OfSender(user_id);
        }

        public Request GetById(int id)
        {
            return repository.Requests.ById(id);
        }

        public Request Insert(Request obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (!current_user.IsAdmin && current_user.ID != obj.SenderID)
                throw new Exception("Unauthorized!");

            repository.Requests.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            repository.Requests.Delete(id);
        }

        public void Accept(int id)
        {
            var obj = repository.Requests.ById(id);
            if (obj == null)
                throw new Exception("Object not found!");

            if (!current_user.IsAdmin && current_user.ID != obj.RecipientID)
                throw new Exception("Unauthorized!");

            if (obj.Type == RequestType.Friendship)
                repository.Users.AddFriend(obj.SenderID, obj.RecipientID);

            // Der Recipient will bestätigen, dass der Sender eine Herausforderung geschafft hat.
            if (obj.Type == RequestType.ChallengeAcceptance)
            {
                repository.Challenges.AddAcceptance(obj.ChallengeID.Value, obj.SenderID);
            }

            repository.Requests.Delete(id);

            // ToDo: Erzeugen einer Neuigkeit!
        }

        public void Deny(int id)
        {
            var obj = repository.Requests.ById(id);
            if (obj == null)
                throw new Exception("Object not found!");

            if (!current_user.IsAdmin && current_user.ID != obj.RecipientID)
                throw new Exception("Unauthorized!");

            repository.Requests.Delete(id);

            // ToDo: Erzeugen einer Neuigkeit?
        }
    }
}