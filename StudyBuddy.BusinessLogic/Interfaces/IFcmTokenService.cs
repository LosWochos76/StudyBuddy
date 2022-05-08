using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IFcmTokenService
    {
        IEnumerable<FcmToken> GetAll();
        IEnumerable<FcmToken> GetForUser(int userId);
        FcmToken Save(FcmTokenSaveDto fcmToken);
    }
}