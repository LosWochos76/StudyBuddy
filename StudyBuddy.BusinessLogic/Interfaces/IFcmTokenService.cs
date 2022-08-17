using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IFcmTokenService
    {
        FcmTokenList GetAll(FcmTokenFilter filter);
        FcmToken Save(FcmTokenSaveDto fcmToken);
        void DeleteOldTokens();
    }
}