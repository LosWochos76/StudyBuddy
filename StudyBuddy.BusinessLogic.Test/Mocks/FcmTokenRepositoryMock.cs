using StudyBuddy.Model;
using StudyBuddy.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.BusinessLogic.Test.Mocks
{
    public class FcmTokenRepositoryMock : IFcmTokenRepository
    {
        private List<FcmToken> _fcmtokens = new();

        public void DeleteOldTokens()
        {
            _fcmtokens.RemoveAll(obj => obj.LastSeen < DateTime.Now);
        }

        public IEnumerable<FcmToken> GetAll(FcmTokenFilter filter)
        {
            return _fcmtokens;
        }

        public int GetCount(FcmTokenFilter filter)
        {
            return _fcmtokens.Count;
        }

        public IEnumerable<FcmToken> GetForUser(int user_id)
        {
            return _fcmtokens.Where(obj => obj.UserID.Equals(user_id));
        }

        public FcmToken Save(FcmToken obj)
        {
            _fcmtokens.Add(obj);

            return obj;
        }
    }
}
