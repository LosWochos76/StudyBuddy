using StudyBuddy.Model;
using StudyBuddy.Model.Filter;
using StudyBuddy.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class RequestRepositoryMock : IRequestRepository
    {
        private List<Request> _requests = new();
        public IEnumerable<Request> All(RequestFilter filter)
        {
            return _requests;
        }

        public Request ById(int id)
        {
            return _requests.Where(obj => obj.ID.Equals(id)).FirstOrDefault();
        }

        public void Delete(int id)
        {
            var obj = _requests.Where(obj => obj.ID== id).FirstOrDefault(); 

            _requests.Remove(obj);
        }

        public Request FindFriendshipRequest(int sender_id, int recipient_id)
        {
            throw new NotImplementedException();
        }

        public Request FindSimilar(Request request)
        {
            return _requests.Where(obj => obj.SenderID.Equals(request.SenderID) && obj.RecipientID.Equals(request.SenderID)
            && obj.Type.Equals(request.Type)
            && obj.ChallengeID.Equals(request.ChallengeID) ? request.Type == RequestType.ChallengeAcceptance : true).FirstOrDefault();
        }

        public int GetCount(RequestFilter filter)
        {
            return _requests.Count;
        }

        public void Insert(Request obj)
        {
            if (obj.ID == 0)
            {
                obj.ID = _requests.Count + 1;
            }
            _requests.Add(obj);
        }
    }
}
