using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IFcmTokenRepository
    {
        IEnumerable<FcmToken> GetAll(FcmTokenFilter filter);
        IEnumerable<FcmToken> GetForUser(int user_id);
        int GetCount(FcmTokenFilter filter);
        FcmToken Save(FcmToken obj);
        void DeleteOldTokens();
    }
}