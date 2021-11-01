using System;

namespace StudyBuddy.Model
{
    public class ChallengeFilter
    {
        public int CurrentUserId { get; set; } = 0;
        public int Start { get; set; } = 0;
        public int Count { get; set; } = 1000;
        public string SearchText { get; set; } = string.Empty;
        public bool WithTags { get; set; } = false;
        public bool OnlyUnacceped { get; set; } = false;
        public int? OwnerId { get; set; } = null;
        public DateTime? ValidAt { get; set; } = null;
    }
}