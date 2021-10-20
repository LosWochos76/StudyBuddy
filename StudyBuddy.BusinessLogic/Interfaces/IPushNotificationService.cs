namespace StudyBuddy.BusinessLogic
{
    public interface IPushNotificationService
    {
        void BroadcastMessage(PushNotificationBroadcastDto obj);
    }
}