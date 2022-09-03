using System;

namespace StudyBuddy.Model
{
    public class AcceptChallengeByLocationResultDTO
    {
        public GeoCoordinate UserPosition { get; set; }
        public GeoCoordinate TargetPosition { get; set; }
        public bool Success { get; set; }
    }
}