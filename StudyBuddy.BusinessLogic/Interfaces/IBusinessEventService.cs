namespace StudyBuddy.BusinessLogic
{
    public interface IBusinessEventService
    {
        void TriggerEvent(object sender, BusinessEventArgs args);
    }
}