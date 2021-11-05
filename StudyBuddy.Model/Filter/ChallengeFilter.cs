using System;

namespace StudyBuddy.Model
{
    public class ChallengeFilter : BaseFilter
    {
        public int CurrentUserId { get; set; } = 0;
        public string SearchText { get; set; } = string.Empty;
        public bool WithTags { get; set; } = false;
        public bool OnlyUnacceped { get; set; } = false;
        public int? OwnerId { get; set; } = null;
        public DateTime? ValidAt { get; set; } = null;
    }
}