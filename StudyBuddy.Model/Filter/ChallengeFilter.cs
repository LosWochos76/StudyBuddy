using System;

namespace StudyBuddy.Model
{
    public class ChallengeFilter : BaseFilter
    {
        public string SearchText { get; set; } = string.Empty;
        public int? OwnerId { get; set; } = null;
        public DateTime? ValidAt { get; set; } = null;

        public bool includeSystemProve { get; set; } = true;

        // if we only want to see accepted/unacepted challenges, we need to pass the user-id as well
        public bool OnlyUnacceped { get; set; } = false;
        public bool OnlyAccepted { get; set; } = false;
        public int CurrentUserId { get; set; } = 0;
        
        public bool WithTags { get; set; } = false;
        public bool WithOwner { get; set; } = false;
    }
}