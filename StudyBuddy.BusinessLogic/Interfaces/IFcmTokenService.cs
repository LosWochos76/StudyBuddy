using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IFcmTokenService
    {
        IEnumerable<FcmToken> All();
        FcmToken Save(FcmTokenSaveDto fcmToken);
    }
}