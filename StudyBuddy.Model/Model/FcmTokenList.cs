using System.Collections.Generic;

namespace StudyBuddy.Model
{
    public class FcmTokenList
    {
        public int Count { get; set; }
        public IEnumerable<FcmToken> Objects { get; set; }
    }
}
