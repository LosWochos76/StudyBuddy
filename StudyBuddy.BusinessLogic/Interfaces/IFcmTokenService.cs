using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public interface IFcmTokenService
    {
        FcmTokenList GetAll(FcmTokenFilter filter);
        FcmToken Save(FcmToken fcmToken);
        void DeleteOldTokens();
    }
}