using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddy.Model.Model
{
    public class UserRanking
    {
        public IEnumerable<UserRankingStruct> OverallRanking{ get; set; }

    }

    public struct UserRankingStruct
    {
        public int User_id { get; set; }
        public string Nickname { get; set; }
        public int Total_Points { get; set; }
        public int Total_Rank { get; set; }
    }
}
