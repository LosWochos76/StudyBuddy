using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddy.Model
{
    public class GameObjectStatistics : Entity
    {
        public int UserID { get; set; }

        //ItemID represents either a challenge or gamebadge ID depending on the context
        public int ItemID { get; set;}
        public DateTime DateCompleted { get; set; }
    }
}
