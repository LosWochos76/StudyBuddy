using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public interface IFcmTokenRepository
    {
        IEnumerable<FcmToken> All(int from = 0, int max = 1000);

        FcmToken Save(FcmToken obj);

        IEnumerable<FcmToken> OfUser(int user_id, int from = 0, int max = 1000);

    }
}