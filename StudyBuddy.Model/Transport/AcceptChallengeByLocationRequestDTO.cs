namespace StudyBuddy.Model
{
    public class AcceptChallengeByLocationRequestDTO
    {
        public int ChallengeID { get; set; }
        public GeoCoordinate UserPosition { get; set; }
    }
}