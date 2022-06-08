using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IFcmTokenRepository
    {
        IEnumerable<FcmToken> GetAll(int from = 0, int max = 1000);

        FcmToken Save(FcmToken obj);

        IEnumerable<FcmToken> ForUser(int user_id, int from = 0, int max = 1000);

        void DeleteOldTokens();
    }
}