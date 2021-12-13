namespace StudyBuddy.Model
{
    public class PersistentImage : Entity
    {
        public int? UserID { get; set; }
        public int? BadgeId { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public byte[] Content { get; set; }
    }
}