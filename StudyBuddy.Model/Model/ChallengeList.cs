﻿using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class ChallengeList
    {
        public int Count { get; set; }
        public IEnumerable<Challenge> Objects { get; set; }
    }
}