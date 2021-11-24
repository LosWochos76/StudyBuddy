namespace StudyBuddy.Model
{
    public class BusinessEvent : Entity
    {
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public BusinessEventType Type { get; set; }
        public string Code { get; set; }
    }
}