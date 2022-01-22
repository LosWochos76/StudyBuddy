﻿using System;
using System.Collections.Generic;
using StudyBuddy.Model;
using StudyBuddy.Model.Filter;

namespace StudyBuddy.BusinessLogic
{
    class RequestService : IRequestService
    {
        private readonly IBackend backend;

        public RequestService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<Request> All(RequestFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new Exception("Unauthorized!");

            if ((!filter.OnlyForRecipient.HasValue || !filter.OnlyForRecipient.HasValue) && !backend.CurrentUser.IsAdmin)
                throw new Exception("Unauthorized!");

            if (filter == null)
                filter = new RequestFilter();

            var result = backend.Repository.Requests.All(filter);

            // ToDo: Muss man beobachten, ob das hier performant genug ist, oder man besser direkt im SQL umsetzt
            foreach (var request in result)
            {
                if (filter.WithSender)
                    request.Sender = backend.Repository.Users.ById(request.SenderID);

                if (filter.WithRecipient)
                    request.Recipient = backend.Repository.Users.ById(request.RecipientID);

                if (filter.WithChallenge && request.Type == RequestType.ChallengeAcceptance && request.ChallengeID.HasValue)
                    request.Challenge = backend.Repository.Challenges.ById(request.ChallengeID.Value);
            }

            return result;
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

            var other = backend.Repository.Requests.FindSimilar(obj);
            if (other != null)
                return other;

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
                backend.BusinessEventService.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.FriendAdded, friend) { CurrentUser = user });
            }
            else if (obj.Type == RequestType.ChallengeAcceptance)
            {
                backend.Repository.Challenges.AddAcceptance(obj.ChallengeID.Value, obj.SenderID);
                var challenge = backend.Repository.Challenges.ById(obj.ChallengeID.Value);
                backend.BusinessEventService.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.ChallengeAccepted, challenge) { CurrentUser = user });
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