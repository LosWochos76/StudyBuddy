using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class RequestService
    {
        private readonly IRepository repository;

        public RequestService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Request> All()
        {
            return repository.Requests.All();
        }

        public IEnumerable<Request> ForRecipient(User current_user, int id)
        {
            if (!current_user.IsAdmin && current_user.ID != id)
                throw new Exception("Unauthorized!");

            return repository.Requests.ForRecipient(id);
        }

        public Request GetById(int id)
        {
            return repository.Requests.ById(id);
        }

        public Request Insert(User current_user, Request obj)
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

        public void Accept(User current_user, int id)
        {
            var obj = repository.Requests.ById(id);
            if (obj == null)
                throw new Exception("Object not found!");

            if (!current_user.IsAdmin && current_user.ID != obj.RecipientID)
                throw new Exception("Unauthorized!");

            if (obj.Type == RequestType.Friendship) repository.Users.AddFriend(obj.SenderID, obj.RecipientID);

            if (obj.Type == RequestType.ChallengeAcceptance)
            {
                // Der Nutzer will bestätigen, dass ein anderer Nutzer eine Herausforderung geschafft hat.
            }

            repository.Requests.Delete(id);

            // ToDo: Erzeugen einer Neuigkeit!
        }
    }
}